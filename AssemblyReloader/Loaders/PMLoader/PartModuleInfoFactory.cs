﻿using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Providers.ConfigNodeProviders;
using AssemblyReloader.Queries.ConfigNodeQueries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleInfoFactory : IPartModuleInfoFactory
    {
        private readonly IPartConfigProvider _configProvider;
        private readonly IModuleConfigsFromPartConfigQuery _moduleConfigQuery;
        private readonly ILog _log;


        public PartModuleInfoFactory(
            IPartConfigProvider configProvider,
            IModuleConfigsFromPartConfigQuery moduleConfigQuery,
            ILog log)
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (moduleConfigQuery == null) throw new ArgumentNullException("moduleConfigQuery");
            if (log == null) throw new ArgumentNullException("log");

            _configProvider = configProvider;
            _moduleConfigQuery = moduleConfigQuery;
            _log = log;
        }


        private IEnumerable<PartModuleInfo> CreatePartModuleInfo(Part prefab, ConfigNode partConfig, Type pmType)
        {
            // make sure there aren't already PartModules on this prefab: that would indicate either a
            // problem with unloading the previous version or that somehow KSP has found a match and we shouldn't
            // mess with it
            if (prefab.GetComponents<PartModule>().Any(pm => pm.GetType().Name == pmType.Name))
            {
                _log.Warning(
                    "Found existing PartModule called {0} on prefab {1}; {2} will not be added to this prefab",
                    pmType.Name, prefab.partInfo.name, pmType.FullName);
                return new PartModuleInfo[] {};
            }

            return
                _moduleConfigQuery.Get(partConfig, pmType.Name)
                    .Select(config => new PartModuleInfo(prefab, config, pmType));
        }


        public IEnumerable<PartModuleInfo> Create(Type pmType)
        {
            if (pmType == null) throw new ArgumentNullException("pmType");
            if (!typeof (PartModule).IsAssignableFrom(pmType))
                throw new ArgumentException("pmType must be derived from PartModule");

            var infoList = new List<PartModuleInfo>();

            PartLoader.LoadedPartsList.ForEach(ap =>
            {
                var partConfig = _configProvider.Get(ap);
                if (!partConfig.Any()) return;

                infoList.AddRange(CreatePartModuleInfo(ap.partPrefab, partConfig.Single(), pmType));
            });

            return infoList;
        }
    }
}
