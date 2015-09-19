extern alias KSP;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetConfigNodeForPart
    {
        Maybe<KSP::ConfigNode> Get(IAvailablePart availablePart);
    }
}
