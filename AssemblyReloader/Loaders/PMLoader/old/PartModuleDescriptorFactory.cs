using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.ConfigNodeQueries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleDescriptorFactory : IDescriptorFactory
    {
        private readonly IPartLoader _partLoader;
        private readonly IAvailablePartConfigProvider _configProvider;
        private readonly IModuleConfigsFromPartConfigQuery _moduleConfigQuery;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;
        private readonly ILog _log;


        public PartModuleDescriptorFactory(
            IPartLoader partLoader,
            IAvailablePartConfigProvider configProvider,
            IModuleConfigsFromPartConfigQuery moduleConfigQuery,
            ITypeIdentifierQuery typeIdentifierQuery,
            ILog log)
        {
            if (partLoader == null) throw new ArgumentNullException("partLoader");
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (moduleConfigQuery == null) throw new ArgumentNullException("moduleConfigQuery");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (log == null) throw new ArgumentNullException("log");

            _partLoader = partLoader;
            _configProvider = configProvider;
            _moduleConfigQuery = moduleConfigQuery;
            _typeIdentifierQuery = typeIdentifierQuery;
            _log = log;
        }


        // Note: returns IEnumerable because there may be multiple duplicate PartModules on one part
        private IEnumerable<PartModuleDescriptor> CreatePartModuleInfo(IPart prefab, ConfigNode partConfig, Type pmType)
        {
            // make sure there aren't already PartModules on this prefab: that would indicate either a
            // problem with unloading the previous version or that somehow KSP has found a match and we shouldn't
            // mess with it
            if (prefab.GameObject.GetComponents<PartModule>().Any(pm => pm.GetType().Name == pmType.Name))
            {
                _log.Warning(
                    "Found existing PartModule called {0} on prefab {1}; {2} will not be added to this prefab",
                    pmType.Name, prefab.PartInfo.Name, pmType.FullName);
                return new PartModuleDescriptor[] {};
            }

            return
                _moduleConfigQuery.Get(partConfig, pmType.Name)
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
