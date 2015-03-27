using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.Providers.Game
{
    public interface IAvailablePartConfigProvider
    {
        Maybe<ConfigNode> Get(IAvailablePart availablePart);
    }
}
