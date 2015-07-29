﻿using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Unsorted;

namespace AssemblyReloader.ReloadablePlugin.Loaders.old
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
        private readonly IAvailablePartConfigQuery _configQuery;
        private readonly IModuleConfigsFromPartConfigQuery _moduleConfigQuery;
        private readonly IGetTypeIdentifier _getTypeIdentifier;


        public PartModuleDescriptorFactory(
            IPartLoader partLoader,
            IAvailablePartConfigQuery configQuery,
            IModuleConfigsFromPartConfigQuery moduleConfigQuery,
            IGetTypeIdentifier getTypeIdentifier)
        {
            if (partLoader == null) throw new ArgumentNullException("partLoader");
            if (configQuery == null) throw new ArgumentNullException("configQuery");
            if (moduleConfigQuery == null) throw new ArgumentNullException("moduleConfigQuery");
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");

            _partLoader = partLoader;
            _configQuery = configQuery;
            _moduleConfigQuery = moduleConfigQuery;
            _getTypeIdentifier = getTypeIdentifier;
        }


        // Note: returns IEnumerable because there may be multiple duplicate PartModules on one part
        private IEnumerable<PartModuleDescriptor> CreatePartModuleInfo(IPart prefab, ConfigNode partConfig, Type pmType)
        {
            return
                _moduleConfigQuery.Get(partConfig, _getTypeIdentifier.Get(pmType).Identifier)
                    .Select(config => new PartModuleDescriptor(prefab, config, pmType, _getTypeIdentifier.Get(pmType)));
        }



        public IEnumerable<PartModuleDescriptor> Create(Type pmType)
        {
            if (pmType == null) throw new ArgumentNullException("pmType");
            if (!typeof (PartModule).IsAssignableFrom(pmType))
                throw new ArgumentException("pmType must be derived from PartModule");

            var infoList = new List<PartModuleDescriptor>();

            _partLoader.LoadedParts.ForEach(ap =>
            {
                var partConfig = _configQuery.Get(ap);
                if (!partConfig.Any()) return;

                infoList.AddRange(CreatePartModuleInfo(ap.PartPrefab, partConfig.Single(), pmType));
            });

            return infoList;
        }
    }
}
