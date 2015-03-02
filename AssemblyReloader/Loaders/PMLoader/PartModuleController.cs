using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleController : IPartModuleController
    {
        private readonly IPartModuleLoader _partModuleLoader;
        private readonly ITypesFromAssemblyQuery _partModuleQuery;
        private readonly IDescriptorFactory _descriptorFactory;
        private readonly ICurrentSceneIsFlightQuery _isFlightQuery;
        private List<IDisposable> _loadedPartModules; 

        public PartModuleController(
            IPartModuleLoader partModuleLoader,
            ITypesFromAssemblyQuery partModuleQuery,
            IDescriptorFactory descriptorFactory,
            ICurrentSceneIsFlightQuery isFlightQuery)
        {
            if (partModuleLoader == null) throw new ArgumentNullException("partModuleLoader");
            if (partModuleQuery == null) throw new ArgumentNullException("partModuleQuery");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (isFlightQuery == null) throw new ArgumentNullException("isFlightQuery");

            _partModuleLoader = partModuleLoader;
            _partModuleQuery = partModuleQuery;
            _descriptorFactory = descriptorFactory;
            _isFlightQuery = isFlightQuery;
        }


        public void LoadPartModules(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var partModuleTypes = _partModuleQuery.Get(assembly);
            var descriptors = partModuleTypes.SelectMany(t => _descriptorFactory.Create(t));

            _loadedPartModules = descriptors.SelectMany(descriptor => _partModuleLoader.Load(descriptor, _isFlightQuery.Get())).ToList();
        }


        public void UnloadPartModules(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (_loadedPartModules.IsNull())
                throw new InvalidOperationException("Assembly must be loaded before PartModules can be unloaded");

            _loadedPartModules.ForEach(d => d.Dispose());
            _loadedPartModules.Clear();
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






        //private void LoadPartModuleIntoPrefab(PartModuleDescriptor descriptor)
        //{
        //    _log.Debug("Adding {0} to {1}", descriptor.Type.FullName, descriptor.Prefab.Name);

        //    var pm = _partModuleLoader.Create(descriptor);

        //    if (pm.IsNull())
        //    {
        //        _log.Warning("Failed to add PartModule " + descriptor.Type.FullName);
        //        return;
        //    }


        //    _log.Verbose("Added {0} to Part {1}", descriptor.Type.FullName, descriptor.Prefab.Name);

        //    if (_flightQuery.Get()) LoadPartModuleIntoFlight(descriptor);
        //}


        //private void LoadPartModuleIntoFlight(PartModuleDescriptor descriptor)
        //{
        //    // todo
        //    _log.Warning("LoadPartModulesIntoFlight: not implemented");
        //}


        //private void UnloadPartModuleFromPrefab(PartModuleDescriptor descriptor)
        //{
        //    _log.Debug("Unloading " + descriptor.Type.FullName + " from prefab " + descriptor.Prefab.Name);

        //    var pm = descriptor.Prefab.GameObject.GetComponent(descriptor.Type) as PartModule;

        //    if (pm.IsNull())
        //    {
        //        _log.Warning("Failed to find prefab " + descriptor.Type.FullName + " on prefab " +
        //                     descriptor.Prefab.PartName);
        //        return;
        //    }

        //    _destructionMediator.InformComponentOfDestruction(pm);

        //    descriptor.Prefab.RemoveModule(pm);
        //    UnityEngine.Object.Destroy(pm);
        //}


        //private void UnloadPartModuleFromFlight(PartModuleDescriptor descriptor)
        //{
        //    _log.Debug("Unloading flight PartModule " + descriptor.Type.FullName + " from flight instances");


        //}


        //public void LoadPartModuleTypes(Assembly assembly)
        //{
        //    if (assembly == null) throw new ArgumentNullException("assembly");

        //    var partModules = _partModuleQuery.Get(assembly).ToList();

        //    _log.Debug(string.Format("Found {0} part module types in assembly", partModules.Count));

        //    _prefabPartModules = partModules.SelectMany(ty => _descriptorFactory.Create(ty)).ToList();

        //    _log.Debug(string.Format("Found {0} part module descriptors", _prefabPartModules.Count));
            
        //    foreach (var info in _prefabPartModules)
        //        LoadPartModuleIntoPrefab(info);
        //}


        //public void ClearPartModuleTypes()
        //{
        //    _log.Debug("Clearing PartModule types");

        //    if (_flightQuery.Get())
        //        foreach (var descriptor in _prefabPartModules)
        //            UnloadPartModuleFromFlight(descriptor);

        //    foreach (var descriptor in _prefabPartModules)
        //        UnloadPartModuleFromPrefab(descriptor);
        //}

    }
}
