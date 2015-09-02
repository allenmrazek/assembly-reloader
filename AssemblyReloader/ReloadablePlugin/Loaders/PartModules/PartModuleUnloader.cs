extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class PartModuleUnloader : IPartModuleUnloader
    {
        private readonly IGetTypesDerivedFrom<KSP::PartModule> _partModuleTypeQuery;
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IGetPartPrefabClones _loadedInstancesOfPrefabQuery;
        private readonly IPartModuleDestroyer _partModuleDestroyer;
        private readonly ILog _log;


        public PartModuleUnloader(
            IGetTypesDerivedFrom<KSP::PartModule> partModuleTypeQuery,
            IPartModuleDescriptorFactory descriptorFactory,
            IGetPartPrefabClones loadedInstancesOfPrefabQuery,
            IPartModuleDestroyer partModuleDestroyer,
            [Name(LogKey.PartModuleUnloader)] ILog log)
        {
            if (partModuleTypeQuery == null) throw new ArgumentNullException("partModuleTypeQuery");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (loadedInstancesOfPrefabQuery == null) throw new ArgumentNullException("loadedInstancesOfPrefabQuery");
            if (partModuleDestroyer == null) throw new ArgumentNullException("partModuleDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleTypeQuery = partModuleTypeQuery;
            _descriptorFactory = descriptorFactory;
            _loadedInstancesOfPrefabQuery = loadedInstancesOfPrefabQuery;
            _partModuleDestroyer = partModuleDestroyer;
            _log = log;
        }


        public void Unload(ILoadedAssemblyHandle handle, bool prefabOnly)
        {
            if (handle == null) throw new ArgumentNullException("handle");
 
            var descriptors = _partModuleTypeQuery
                .Get(handle.LoadedAssembly.assembly)
                .SelectMany(partModuleType => _descriptorFactory.Create(partModuleType))
                .ToList();

            foreach (var descriptor in descriptors)
            {
                var loadedInstances = _loadedInstancesOfPrefabQuery.Get(descriptor.Prefab);
                var prefab = descriptor.Prefab;
                var allInstances = loadedInstances.Union(new[] {prefab});

                DestroyPartModulesWithDescriptor(allInstances, descriptor);
            }

            _log.Debug("Unloaded " + descriptors.Count + " PartModule descriptors");
        }


        private void DestroyPartModulesWithDescriptor(IEnumerable<IPart> targets, PartModuleDescriptor descriptor)
        {
            if (targets == null) throw new ArgumentNullException("targets");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            var targetList = targets.ToList();

            if (!targetList.Any()) // why is there a descriptor if no PartModule instances use it? something has broken!
                _log.Warning("No PartModules with descriptor " + descriptor + " found");

            foreach (var targetPart in targetList)
            {
                var partModule = targetPart.GameObject.GetComponent(descriptor.Type) as KSP::PartModule;

                if (partModule == null)
                {
                    // could be a problem... might indicate an error in last load
                    _log.Verbose("Did not find component on part " + targetPart.FlightID + ", " + descriptor);
                    continue;
                }

                _partModuleDestroyer.Destroy(targetPart, partModule);
            }
        }
    }
}
