using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.ConfigNodeQueries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
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
            return
                _moduleConfigQuery.Get(partConfig, _typeIdentifierQuery.Get(pmType).Identifier)
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
