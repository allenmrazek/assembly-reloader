using System;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleConfigNodeRepository
    {
        void Store(TypeIdentifier identifier, Guid vessel, ConfigNode config);
        Maybe<ConfigNode> Retrieve(TypeIdentifier smType, Guid vessel);
        void Clear();
        int Count();
    }
}
