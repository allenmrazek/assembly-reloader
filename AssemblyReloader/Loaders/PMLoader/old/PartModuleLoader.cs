//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AssemblyReloader.Providers;
//using AssemblyReloader.Repositories;

//namespace AssemblyReloader.Loaders.PMLoader
//{
//    public class PartModuleLoader : IPartModuleLoader
//    {
//        private readonly IPartModuleFactory _partModuleFactory;
//        private readonly ILoadedPartModuleHandleFactory _handleFactory;
//        private readonly IPartModuleFlightConfigRepository _flightConfigs;
//        private readonly ILoadedInstancesOfPrefabProvider _loadedPrefabProvider;


//        public PartModuleLoader(
//            IPartModuleFactory partModuleFactory,
//            ILoadedPartModuleHandleFactory handleFactory,
//            IPartModuleFlightConfigRepository flightConfigs,
//            ILoadedInstancesOfPrefabProvider loadedPrefabProvider)
//        {
//            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
//            if (handleFactory == null) throw new ArgumentNullException("handleFactory");
//            if (flightConfigs == null) throw new ArgumentNullException("flightConfigs");
//            if (loadedPrefabProvider == null) throw new ArgumentNullException("loadedPrefabProvider");

//            _partModuleFactory = partModuleFactory;
//            _handleFactory = handleFactory;
//            _flightConfigs = flightConfigs;
//            _loadedPrefabProvider = loadedPrefabProvider;
//        }


//        public IEnumerable<IDisposable> Load(PartModuleDescriptor descriptor, bool inFlight)
//        {
//            if (descriptor == null) throw new ArgumentNullException("descriptor");

//            var handle = _handleFactory.Create(_partModuleFactory.Create(descriptor.Type, descriptor.Prefab, descriptor.Config));

//            if (!inFlight)
//                return new []{handle};

//            // if we're in flight, try to use any previously saved flight config if possible
//            var loadedParts = _loadedPrefabProvider.Get(descriptor.Prefab);

//            var createdItems = loadedParts.Select(lp =>
//            {
//                var config = descriptor.Config;

//                if (_flightConfigs.Peek(lp.FlightID, descriptor.Identifier).Any())
//                    config = _flightConfigs.Retrieve(lp.FlightID, descriptor.Identifier).Single();

//                return _partModuleFactory.Create(descriptor.Type, lp, config);
//            });
            
//            return createdItems.Select(kvp => _handleFactory.Create(kvp)).Concat(new[]{handle});
//        }
//    }
//}
