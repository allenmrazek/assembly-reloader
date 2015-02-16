using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Repositories;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleLoader : IDisposable
    {
        private readonly IEnumerable<Type> _partModules;
        private readonly IPartModuleFlightConfigRepository _flightConfigs;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IPartModuleInfoFactory _pmiFactory;
        private readonly ILog _log;

        private readonly List<PartModuleInfo> _createdPrefabPartModules = new List<PartModuleInfo>();
 
        public PartModuleLoader(
            IEnumerable<Type> partModules, 

            
            IPartModuleFlightConfigRepository flightConfigs,

            IPartModuleFactory partModuleFactory,
            IPartModuleInfoFactory pmiFactory,

            ILog log)
        {
            if (partModules == null) throw new ArgumentNullException("partModules");
            if (flightConfigs == null) throw new ArgumentNullException("flightConfigs");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (pmiFactory == null) throw new ArgumentNullException("pmiFactory");
            if (log == null) throw new ArgumentNullException("log");

            _partModules = partModules;
            _flightConfigs = flightConfigs;
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
                DestroyPartModulesOnLoadedParts();
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


        /// <summary>
        /// For instances of a particular PartModule (which only exist in flight, for loaded vessels), we
        /// want to preserve ConfigNode data for cases where stateful info is being saved. That info must
        /// be captured so it can be passed directly to the new PartModule.
        /// 
        /// This only matters in flight for loaded PMs; at any other time, KSP will handle this automatically
        /// </summary>
        private void DestroyPartModulesOnLoadedParts()
        {
            _flightConfigs.Clear(); // old contents no longer relevant

            _log.Debug("Destroying PartModules on loaded Parts");



            foreach (var pmi in _createdPrefabPartModules)
            {
                _log.Debug("Found {0} instances to destroy", GetLoadedInstancesOfPart(pmi.Prefab).Count().ToString());

                foreach (var part in GetLoadedInstancesOfPart(pmi.Prefab))
                {
                    var pm = part.gameObject.GetComponent(pmi.PmType) as PartModule;
                    if (pm.IsNull())
                    {
                        _log.Warning("Failed to GetComponent {0} on part {1} (vessel {2})", pmi.Identifier,
                            part.flightID.ToString(CultureInfo.InvariantCulture), part.vessel.vesselName);
                        continue;
                    }

                    _log.Debug("Creating PartModule config snapshot for {0} on {1}", pmi.Identifier,
                        part.flightID.ToString());
                    CreateConfigSnapshot(part, pm, pmi);

                    UnityEngine.Object.DestroyImmediate(pm);
                }
            }

            _log.Debug("Finished destroying PartModules on loaded Parts");
        }


        public void LoadPartModulesIntoPrefabs()
        {
            _log.Verbose("Begin loading PartModules into prefab GameObjects");

            foreach (var pm in _partModules)
                LoadPartModuleIntoPrefab(_pmiFactory.Create(pm));

            _log.Verbose("Finished loading PartModules into prefabs");
        }


        public void LoadPartModulesIntoFlight()
        {
            _log.Verbose("Begin loading PartModules into existing flight GameObjects");

            LoadPartModulesIntoFlight(_createdPrefabPartModules);

            _log.Verbose("Finished loading PartModules into flight objects");
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


        private void LoadPartModulesIntoFlight(IEnumerable<PartModuleInfo> infoList)
        {
            foreach (var info in infoList)
                LoadPartModuleIntoFlightParts(info, GetLoadedInstancesOfPart(info.Prefab));
        }


        private void LoadPartModuleIntoFlightParts(PartModuleInfo info, IEnumerable<Part> parts)
        {
            throw new NotImplementedException();
            //if (info == null) throw new ArgumentNullException("info");
            //if (parts == null) throw new ArgumentNullException("parts");

            //foreach (var part in parts)
            //{
            //    var config = _flightConfigs.Retrieve(part.flightID, info.Identifier).Or(info.Config);
            //    CreatePartModule(part, info.PmType, config);
            //}
        }





        private IEnumerable<Part> GetLoadedInstancesOfPart(Part prefab)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");

            // locate any loaded vessels that contain a part which matches this prefab
            ILoadedVesselProvider loadedVessels = new LoadedVesselProvider();

            _log.Debug("Found " + loadedVessels.Get().Count() + " loaded vessels");

            _log.Debug("found " + loadedVessels.Get()
                .SelectMany(v => v.parts)
                .Where(p => ReferenceEquals(p.partInfo.partPrefab, prefab)).Count() + " matches to " + prefab.name
                );

            return loadedVessels.Get()
                .SelectMany(v => v.parts)
                .Where(p => ReferenceEquals(p.partInfo.partPrefab, prefab));
        }


        private void CreateConfigSnapshot(Part part, PartModule pm, PartModuleInfo pmi)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (pm == null) throw new ArgumentNullException("pm");
            if (pmi == null) throw new ArgumentNullException("pmi");

            var config = pmi.Config.CreateCopy();

            pm.OnSave(config);

            _flightConfigs.Store(part.flightID, pmi.Identifier, config);
        }


    }
}
