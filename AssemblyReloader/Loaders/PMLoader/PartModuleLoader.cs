using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Destruction;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Repositories;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader
{
    /// <summary>
    /// Creates descriptors which contain all data needed to create a PartModule, then constructs all
    /// PartModules as needed using a factory
    /// </summary>
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IPartModulesFromAssemblyQuery _partModuleQuery;
        private readonly ICurrentSceneIsFlightQuery _flightQuery;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IDescriptorFactory _descriptorFactory;
        private readonly IDestructionMediator _destructionMediator;
        private readonly IPartModuleFlightConfigRepository _flightConfigs;
        private readonly ILog _log;

        private List<PartModuleDescriptor> _prefabPartModules;
 
        public PartModuleLoader(
            IPartModulesFromAssemblyQuery partModuleQuery,
            ICurrentSceneIsFlightQuery flightQuery,

            IPartModuleFactory partModuleFactory,
            IDescriptorFactory descriptorFactory,

            IDestructionMediator destructionMediator,
            IPartModuleFlightConfigRepository flightConfigs,
            ILog log)
        {
            if (partModuleQuery == null) throw new ArgumentNullException("partModuleQuery");
            if (flightQuery == null) throw new ArgumentNullException("flightQuery");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (flightConfigs == null) throw new ArgumentNullException("flightConfigs");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleQuery = partModuleQuery;
            _flightQuery = flightQuery;
            _partModuleFactory = partModuleFactory;
            _descriptorFactory = descriptorFactory;
            _destructionMediator = destructionMediator;
            _flightConfigs = flightConfigs;
            _log = log;
        }


        ~PartModuleLoader()
        {
            _log.Warning("PartModuleLoader destructing");
        }
 


        //private void DestroyPartModulesOnPrefabs()
        //{
        //    _createdPrefabPartModules.ForEach(descriptor =>
        //    {
        //        var pm = descriptor.Prefab.GetComponent(descriptor.Type);

        //        if (pm.IsNull())
        //        {
        //            _log.Warning("{0} no longer exists on part {1}", descriptor.Type.FullName, descriptor.Prefab.name);
        //            return;
        //        }

        //        _log.Debug("Destroying {0} on {1}", descriptor.Type.FullName, descriptor.Prefab.name);
        //        UnityEngine.Object.DestroyImmediate(pm);
        //    });
        //}






        private void LoadPartModuleIntoPrefab(PartModuleDescriptor descriptor)
        {
            _log.Debug("Adding {0} to {1}", descriptor.Type.FullName, descriptor.Prefab.Name);

            var pm = _partModuleFactory.Create(descriptor);

            if (pm.IsNull())
            {
                _log.Warning("Failed to add PartModule " + descriptor.Type.FullName);
                return;
            }


            _log.Verbose("Added {0} to Part {1}", descriptor.Type.FullName, descriptor.Prefab.Name);

            if (_flightQuery.Get()) LoadPartModuleIntoFlight(descriptor);
        }


        private void LoadPartModuleIntoFlight(PartModuleDescriptor descriptor)
        {
            // todo
            _log.Warning("LoadPartModulesIntoFlight: not implemented");
        }


        private void UnloadPartModuleFromPrefab(PartModuleDescriptor descriptor)
        {
            _log.Debug("Unloading " + descriptor.Identifier + " from prefab " + descriptor.Prefab.Name);

            var pm = descriptor.Prefab.GameObject.GetComponent(descriptor.Type) as PartModule;

            if (pm.IsNull())
            {
                _log.Warning("Failed to find prefab " + descriptor.Identifier + " on prefab " +
                             descriptor.Prefab.PartName);
                return;
            }

            _destructionMediator.InformComponentOfDestruction(pm);

            descriptor.Prefab.RemoveModule(pm);
            UnityEngine.Object.Destroy(pm);
        }


        private void UnloadPartModuleFromFlight(PartModuleDescriptor descriptor)
        {
            _log.Debug("Unloading flight PartModule " + descriptor.Identifier + " from flight instances");


        }


        public void LoadPartModuleTypes(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var partModules = _partModuleQuery.Get(assembly).ToList();

            _log.Debug(string.Format("Found {0} part module types in assembly", partModules.Count));

            _prefabPartModules = partModules.SelectMany(ty => _descriptorFactory.Create(ty)).ToList();

            _log.Debug(string.Format("Found {0} part module descriptors", _prefabPartModules.Count));
            
            foreach (var info in _prefabPartModules)
                LoadPartModuleIntoPrefab(info);
        }


        public void ClearPartModuleTypes()
        {
            _log.Debug("Clearing PartModule types");

            if (_flightQuery.Get())
                foreach (var descriptor in _prefabPartModules)
                    UnloadPartModuleFromFlight(descriptor);

            foreach (var descriptor in _prefabPartModules)
                UnloadPartModuleFromPrefab(descriptor);
        }
    }
}
