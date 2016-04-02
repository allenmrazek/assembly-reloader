using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetConfigNodeForPart
    {
        Maybe<ConfigNode> Get(IAvailablePart availablePart);
    }
}
