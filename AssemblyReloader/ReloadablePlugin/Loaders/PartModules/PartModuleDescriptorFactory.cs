using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class PartModuleDescriptorFactory : IPartModuleDescriptorFactory
    {
        private readonly IPartLoader _partLoader;
        private readonly IGetConfigNodeForPart _configQuery;
        private readonly IGetPartModuleConfigsFromPartConfig _getPartModuleConfig;
        private readonly IGetTypeIdentifier _getTypeIdentifier;


        public PartModuleDescriptorFactory(
            IPartLoader partLoader,
            IGetConfigNodeForPart configQuery,
            IGetPartModuleConfigsFromPartConfig getPartModuleConfig,
            IGetTypeIdentifier getTypeIdentifier)
        {
            if (partLoader == null) throw new ArgumentNullException("partLoader");
            if (configQuery == null) throw new ArgumentNullException("configQuery");
            if (getPartModuleConfig == null) throw new ArgumentNullException("getPartModuleConfig");
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");

            _partLoader = partLoader;
            _configQuery = configQuery;
            _getPartModuleConfig = getPartModuleConfig;
            _getTypeIdentifier = getTypeIdentifier;
        }


        // Note: returns IEnumerable because there may be multiple duplicate PartModules on one part
        private IEnumerable<PartModuleDescriptor> CreatePartModuleInfo(IPart prefab, ConfigNode partConfig, Type pmType)
        {
            var configNodes = _getPartModuleConfig.Get(partConfig, _getTypeIdentifier.Get(pmType).Identifier).ToList();


            return
                _getPartModuleConfig.Get(partConfig, _getTypeIdentifier.Get(pmType).Identifier)
                    .Select(config => new PartModuleDescriptor(prefab, config, pmType, _getTypeIdentifier.Get(pmType)));
        }



        public IEnumerable<PartModuleDescriptor> Create(Type pmType)
        {
            if (pmType == null) throw new ArgumentNullException("pmType");
            if (!typeof (PartModule).IsAssignableFrom(pmType))
                throw new ArgumentException("pmType must be derived from PartModule");

            var infoList = new List<PartModuleDescriptor>();

            foreach (var pmInfo in from ap in _partLoader.LoadedParts 
                                   let partConfig = _configQuery.Get(ap) 
                                   where partConfig.Any() 
                                   select CreatePartModuleInfo(ap.PartPrefab, partConfig.Single(), pmType).ToList() into pmInfo 
                                   where pmInfo.Any() select pmInfo)
                infoList.AddRange(pmInfo);

            return infoList;
        }
    }
}