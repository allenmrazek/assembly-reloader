extern alias KSP;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetConfigNodeForPart
    {
        Maybe<KSP::ConfigNode> Get(IAvailablePart availablePart);
    }
}
