using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleUnloader
    {
        void Unload(ILoadedAssemblyHandle handle);
    }
}
