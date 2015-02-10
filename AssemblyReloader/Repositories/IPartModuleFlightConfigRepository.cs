using ReeperCommon.Containers;

namespace AssemblyReloader.Repositories
{
    public interface IPartModuleFlightConfigRepository
    {
        void Store(uint flightid, string key, ConfigNode data);
        Maybe<ConfigNode> Retrieve(uint flightid, string key);
        void Clear();
    }
}
