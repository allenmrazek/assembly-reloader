﻿using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Providers.Game;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    /// <summary>
    /// Each PartModuleDescriptor wraps relevant data about a particular PartModule instance that
    /// should exist on a particular Part prefab
    /// </summary>
    public class PartModuleDescriptor
    {
        public IPart Prefab { get; private set; }
        public ConfigNode Config { get; private set; }
        public Type Type { get; private set; }
        public ITypeIdentifier Identifier { get; private set; }

        public PartModuleDescriptor(IPart prefab, ConfigNode config, Type type, ITypeIdentifier typeIdentifier)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");
            if (config == null) throw new ArgumentNullException("config");
            if (type == null) throw new ArgumentNullException("type");

            Prefab = prefab;
            Config = config;
            Type = type;
            Identifier = typeIdentifier;
        }

    }

    public class PartModuleDescriptorFactory : IPartModuleDescriptorFactory
    {
        private readonly IPartLoader _partLoader;
        private readonly IAvailablePartConfigProvider _configProvider;
        private readonly IModuleConfigsFromPartConfigProvider _moduleConfigProvider;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;


        public PartModuleDescriptorFactory(
            IPartLoader partLoader,
            IAvailablePartConfigProvider configProvider,
            IModuleConfigsFromPartConfigProvider moduleConfigProvider,
            ITypeIdentifierQuery typeIdentifierQuery)
        {
            if (partLoader == null) throw new ArgumentNullException("partLoader");
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (moduleConfigProvider == null) throw new ArgumentNullException("moduleConfigProvider");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");

            _partLoader = partLoader;
            _configProvider = configProvider;
            _moduleConfigProvider = moduleConfigProvider;
            _typeIdentifierQuery = typeIdentifierQuery;
        }


        // Note: returns IEnumerable because there may be multiple duplicate PartModules on one part
        private IEnumerable<PartModuleDescriptor> CreatePartModuleInfo(IPart prefab, ConfigNode partConfig, Type pmType)
        {
            return
                _moduleConfigProvider.Get(partConfig, _typeIdentifierQuery.Get(pmType).Identifier)
                    .Select(config => new PartModuleDescriptor(prefab, config, pmType, _typeIdentifierQuery.Get(pmType)));
        }



        public IEnumerable<PartModuleDescriptor> Create(Type pmType)
        {
            if (pmType == null) throw new ArgumentNullException("pmType");
            if (!typeof (PartModule).IsAssignableFrom(pmType))
                throw new ArgumentException("pmType must be derived from PartModule");

            var infoList = new List<PartModuleDescriptor>();

            _partLoader.LoadedParts.ForEach(ap =>
            {
                var partConfig = _configProvider.Get(ap);
                if (!partConfig.Any()) return;

                infoList.AddRange(CreatePartModuleInfo(ap.PartPrefab, partConfig.Single(), pmType));
            });

            return infoList;
        }
    }
}