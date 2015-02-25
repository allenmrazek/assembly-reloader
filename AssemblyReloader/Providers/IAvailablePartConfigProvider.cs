using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers
{
    public interface IAvailablePartConfigProvider
    {
        Maybe<ConfigNode> Get(IAvailablePart availablePart);
    }
}
