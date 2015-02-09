using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleLoader : IDisposable
    {
        private readonly IEnumerable<Type> _partModules;
        private readonly IPartModuleInfoFactory _pmiFactory;
        private readonly ILog _log;

        private readonly List<PartModuleInfo> _createdPartModules = new List<PartModuleInfo>();
 
        public PartModuleLoader(
            IEnumerable<Type> partModules, 
            IPartModuleInfoFactory pmiFactory,
            ILog log)
        {
            if (partModules == null) throw new ArgumentNullException("partModules");
            if (pmiFactory == null) throw new ArgumentNullException("pmiFactory");
            if (log == null) throw new ArgumentNullException("log");

            _partModules = partModules;
            _pmiFactory = pmiFactory;
            _log = log;
        }


        ~PartModuleLoader()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
        }


        private void Dispose(bool managed)
        {
            if (managed)
            {
                _createdPartModules.ForEach(pmi =>
                {
                    var pm = pmi.Prefab.GetComponent(pmi.PmType);

                    if (pm.IsNull())
                    {
                        _log.Warning("{0} no longer exists on part {1}", pmi.PmType.FullName, pmi.Prefab.name);
                        return;
                    }

                    _log.Debug("Destroying {0} on {1}", pmi.PmType.FullName, pmi.Prefab.name);
                    UnityEngine.Object.DestroyImmediate(pm);
                });
            }

            GC.SuppressFinalize(this);
        }


        public void LoadPartModulesIntoPrefabs()
        {
            _log.Verbose("Begin loading PartModules into prefab GameObjects");

            foreach (var pm in _partModules)
                LoadPartModuleIntoPrefab(pm);

            _log.Verbose("Finished loading PartModules into prefabs");
        }


        public void LoadPartModulesIntoFlight()
        {
            throw new NotImplementedException();
        }


        private void LoadPartModuleIntoPrefab(Type pmType)
        {
            var infoList = _pmiFactory.Create(pmType).ToList();

            _log.Normal("Found {0} prefab entries for {1}", infoList.Count.ToString(CultureInfo.InvariantCulture), pmType.FullName);

            infoList.ForEach(info =>
            {
                _log.Debug("Adding {0} to {1}", pmType.FullName, info.Prefab.name);

                InsertAndLoadPartModule(info);

                _log.Verbose("Added {0} to Part {1}", pmType.FullName, info.Prefab.name);
            });
        }


        private void InsertAndLoadPartModule(PartModuleInfo pmi)
        {
            var pm = pmi.Prefab.gameObject.AddComponent(pmi.PmType) as PartModule;

            if (pm.IsNull())
            {
                _log.Error("Failed to add {0} to {1}; AddComponent returned null", pmi.PmType.FullName,
                    pmi.Prefab.partInfo.name);
                return;
            }

            pmi.Prefab.Modules.Add(pm);

            _log.Debug("Loading PartModule config");

            // When PartLoader ran through these, the GameObject it was working on had to be awake; otherwise
            // the PartModules wouldn't do some internal setup required. We're going second and the GameObject is
            // already inactive. If we reactivate it, the PartModules will start exceptions. We need to manually
            // force that internal setup from Awake
            typeof(PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                .Invoke(pm, null);

            pm.Load(pmi.Config);
            _createdPartModules.Add(pmi);
        }
    }
}
