﻿using System;
using System.Linq;
using AssemblyReloader.Providers.Game;
using AssemblyReloader.Repositories;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IFlightConfigRepository _partModuleConfigRepository;
        private readonly IPartPrefabCloneProvider _loadedPrefabProvider;


        public PartModuleLoader(
            IPartModuleDescriptorFactory descriptorFactory,
            IPartModuleFactory partModuleFactory,
            IFlightConfigRepository partModuleConfigRepository,
            IPartPrefabCloneProvider loadedPrefabProvider)
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


        public void Load(Type type)
        {
            if (!type.IsSubclassOf(typeof (PartModule)))
                throw new Exception(type.FullName + " is not a subclass of PartModule");
            
            var descriptions = _descriptorFactory.Create(type).ToList();

            descriptions.ForEach(description => LoadPartModule(description));
        }


        private void LoadPartModule(PartModuleDescriptor description)
        {
            _partModuleFactory.Create(description.Prefab, description.Type, description.Config);

            var log = new DebugLog("PartModuleLoader");

            log.Warning("LoadPartModule in PartModuleLoader");
            var items = _loadedPrefabProvider.Get(description.Prefab).ToList();

            log.Warning("Found " + items.Count.ToString() + " prefab instances");

            foreach (var loadedInstance in _loadedPrefabProvider.Get(description.Prefab).ToList())
            {
                log.Normal("Loading PartModule on " + loadedInstance.FlightID);

                var stored = _partModuleConfigRepository.Retrieve(loadedInstance.FlightID, description.Identifier);

                if (stored.Any())
                    log.Normal("*** found stored ConfigNode for this item ***");
                else log.Normal("Didn't find ConfigNode for " + loadedInstance.FlightID);

                var config = stored.Any() ? stored.Single() : description.Config;

                _partModuleFactory.Create(loadedInstance, description.Type, config);
            }
        }
    }
}