using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetVesselFromPartModule
    {
        Maybe<IVessel> Get(PartModule partModule);
    }
}
