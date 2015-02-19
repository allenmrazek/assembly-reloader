using System;
using System.Collections.Generic;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader.old
{
    public class PartModuleLoader : IDisposable
    {
        private readonly IEnumerable<Type> _partModules;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IPartModuleInfoFactory _pmiFactory;
        private readonly ILog _log;

        private readonly List<PartModuleInfo> _createdPrefabPartModules = new List<PartModuleInfo>();
 
        public PartModuleLoader(
            IEnumerable<Type> partModules, 

            IPartModuleFactory partModuleFactory,
            IPartModuleInfoFactory pmiFactory,

            ILog log)
        {
            if (partModules == null) throw new ArgumentNullException("partModules");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (pmiFactory == null) throw new ArgumentNullException("pmiFactory");
            if (log == null) throw new ArgumentNullException("log");

            _partModules = partModules;
            _partModuleFactory = partModuleFactory;
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
                _log.Verbose("Disposing");
                //DestroyPartModulesOnLoadedParts();
                DestroyPartModulesOnPrefabs();
            }

            GC.SuppressFinalize(this);
        }


        private void DestroyPartModulesOnPrefabs()
        {
            _createdPrefabPartModules.ForEach(pmi =>
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





        public void LoadPartModulesIntoPrefabs()
        {
            _log.Verbose("Begin loading PartModules into prefab GameObjects");

            foreach (var pm in _partModules)
                LoadPartModuleIntoPrefab(_pmiFactory.Create(pm));

            _log.Verbose("Finished loading PartModules into prefabs");
        }





        private void LoadPartModuleIntoPrefab(IEnumerable<PartModuleInfo> infoList)
        {
            foreach (var info in infoList)
            {
                _log.Debug("Adding {0} to {1}", info.PmType.FullName, info.Prefab.name);

                var pm = _partModuleFactory.AddPseudoModule(info.PmType, info.Prefab, info.Config, true);

                if (pm.IsNull())
                {
                    _log.Warning("Failed to add PartModule");
                    continue;
                }

                _createdPrefabPartModules.Add(info);

                _log.Verbose("Added {0} to Part {1}", info.PmType.FullName, info.Prefab.name);
            }
        }










    }
}
