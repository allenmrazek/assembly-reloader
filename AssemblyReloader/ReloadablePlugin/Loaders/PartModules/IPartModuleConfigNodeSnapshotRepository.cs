using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleConfigNodeSnapshotRepository
    {
        void Store(uint flightid, ITypeIdentifier key, ConfigNode data);
        Maybe<ConfigNode> Retrieve(uint flightid, ITypeIdentifier key);
        Maybe<ConfigNode> Peek(uint flightid, ITypeIdentifier key);
        void Clear();
    }
}
