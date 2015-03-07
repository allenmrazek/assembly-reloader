using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Repositories;

namespace AssemblyReloader.Loaders
{
    public class PartModuleLoader : IPersistentObjectLoader
    {
        private readonly IDescriptorFactory _descriptorFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IFlightConfigRepository _partModuleConfigRepository;
        private readonly ILoadedInstancesOfPrefabProvider _loadedPrefabProvider;


        public PartModuleLoader(
            IDescriptorFactory descriptorFactory,
            IPartModuleFactory partModuleFactory,
            IFlightConfigRepository partModuleConfigRepository,
            ILoadedInstancesOfPrefabProvider loadedPrefabProvider)
        {
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (partModuleConfigRepository == null) throw new ArgumentNullException("partModuleConfigRepository");
            if (loadedPrefabProvider == null) throw new ArgumentNullException("loadedPrefabProvider");

            _descriptorFactory = descriptorFactory;
            _partModuleFactory = partModuleFactory;
            _partModuleConfigRepository = partModuleConfigRepository;
            _loadedPrefabProvider = loadedPrefabProvider;
        }


        public void Load(Type type, bool inFlight)
        {
            if (!type.IsSubclassOf(typeof (PartModule)))
                throw new Exception(type.FullName + " is not a subclass of PartModule");
            
            var descriptions = _descriptorFactory.Create(type).ToList();

            descriptions.ForEach(description => LoadPartModule(description, inFlight));
        }


        private void LoadPartModule(PartModuleDescriptor description, bool flight)
        {
            _partModuleFactory.Create(description.Prefab, description.Type, description.Config);

            if (flight)
            {
                foreach (var loadedInstance in _loadedPrefabProvider.Get(description.Prefab).ToList())
                {
                    var stored = _partModuleConfigRepository.Retrieve(loadedInstance.FlightID, description.Identifier);
                    var config = stored.Any() ? stored.Single() : description.Config;

                    _partModuleFactory.Create(loadedInstance, description.Type, config);
                }
            }
        }
    }
}
