using AssemblyReloader.Game;
using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleLoader
    {
        void Load(ILoadedAssemblyHandle handle);
    }
}
