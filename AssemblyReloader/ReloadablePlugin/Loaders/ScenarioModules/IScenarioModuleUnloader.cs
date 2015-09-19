using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleUnloader
    {
        void Unload(ILoadedAssemblyHandle handle);
    }
}
