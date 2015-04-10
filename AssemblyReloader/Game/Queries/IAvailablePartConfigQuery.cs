using ReeperCommon.Containers;

namespace AssemblyReloader.Game.Queries
{
    public interface IAvailablePartConfigQuery
    {
        Maybe<ConfigNode> Get(IAvailablePart availablePart);
    }
}
