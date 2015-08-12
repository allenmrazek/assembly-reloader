extern alias KSP;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleConfigNodeSnapshotRepository
    {
        void Store(uint flightid, TypeIdentifier key, KSP::ConfigNode data);
        Maybe<KSP::ConfigNode> Retrieve(uint flightid, TypeIdentifier key);
        Maybe<KSP::ConfigNode> Peek(uint flightid, TypeIdentifier key);
        void Clear();
    }
}
