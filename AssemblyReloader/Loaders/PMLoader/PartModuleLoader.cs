using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Providers;
using AssemblyReloader.Repositories;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IPartModuleFactory _factory;
        private readonly IPartModuleFlightConfigRepository _flightConfigs;
        private readonly ILoadedInstancesOfPrefabProvider _loadedPrefabProvider;


        public PartModuleLoader(
            IPartModuleFactory factory,
            IPartModuleFlightConfigRepository flightConfigs,
            ILoadedInstancesOfPrefabProvider loadedPrefabProvider)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            if (flightConfigs == null) throw new ArgumentNullException("flightConfigs");
            if (loadedPrefabProvider == null) throw new ArgumentNullException("loadedPrefabProvider");

            _factory = factory;
            _flightConfigs = flightConfigs;
            _loadedPrefabProvider = loadedPrefabProvider;
        }


        public IEnumerable<IDisposable> Load(PartModuleDescriptor descriptor, bool inFlight)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            var handle = _factory.Create(descriptor.Type, descriptor.Prefab.GameObject, descriptor.Config);

            if (!inFlight)
                return new []{handle};

  
            var loadedParts = _loadedPrefabProvider.Get(descriptor.Prefab);

            return loadedParts.Select(lp =>
            {
                var config = descriptor.Config;

                if (_flightConfigs.Peek(lp.FlightID, descriptor.Identifier).Any())
                    config = _flightConfigs.Retrieve(lp.FlightID, descriptor.Identifier).Single();

                return _factory.Create(descriptor.Type, lp.GameObject, config);
            }).Concat(new[]{handle});
        }
    }
}
