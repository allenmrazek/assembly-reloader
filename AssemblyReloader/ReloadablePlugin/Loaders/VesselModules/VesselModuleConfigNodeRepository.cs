using System;
using System.Collections.Generic;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    class VesselModuleConfigNodeRepository : DictionaryOfQueues<KeyValuePair<TypeIdentifier, Guid>, ConfigNode>, IVesselModuleConfigNodeRepository 
    {
        public void Store(TypeIdentifier identifier, Guid vessel, ConfigNode config)
        {
            Store(new KeyValuePair<TypeIdentifier, Guid>(identifier, vessel), config);
        }

        public Maybe<ConfigNode> Retrieve(TypeIdentifier smType, Guid vessel)
        {
            return Retrieve(new KeyValuePair<TypeIdentifier, Guid>(smType, vessel));
        }
    }
}
