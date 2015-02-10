using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public interface IPartConfigProvider
    {
        Maybe<ConfigNode> Get(AvailablePart availablePart);
    }
}
