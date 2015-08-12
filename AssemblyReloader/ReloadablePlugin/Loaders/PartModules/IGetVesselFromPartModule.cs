extern alias KSP;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetVesselFromPartModule
    {
        Maybe<IVessel> Get(KSP::PartModule partModule);
    }
}
