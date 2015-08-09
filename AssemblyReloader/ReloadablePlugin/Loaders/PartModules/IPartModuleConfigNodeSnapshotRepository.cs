using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleConfigNodeSnapshotRepository
    {
        void Store(uint flightid, TypeIdentifier key, ConfigNode data);
        Maybe<ConfigNode> Retrieve(uint flightid, TypeIdentifier key);
        Maybe<ConfigNode> Peek(uint flightid, TypeIdentifier key);
        void Clear();
    }
}
