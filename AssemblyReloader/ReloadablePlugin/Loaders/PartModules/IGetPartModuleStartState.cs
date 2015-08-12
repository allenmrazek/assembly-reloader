extern alias KSP;
using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetPartModuleStartState
    {
        KSP::PartModule.StartState Get(Maybe<IVessel> vessel);
    }
}
