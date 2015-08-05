using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetPartModuleStartState
    {
        PartModule.StartState Get(Maybe<IVessel> vessel);
    }
}
