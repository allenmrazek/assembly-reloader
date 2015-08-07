using AssemblyReloader.Game;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IGetCurrentGame
    {
        Maybe<IGame> Get();
    }
}
