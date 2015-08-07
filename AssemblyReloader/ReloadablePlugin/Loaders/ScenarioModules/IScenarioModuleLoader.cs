﻿using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleLoader
    {
        void Load(ILoadedAssemblyHandle handle);
    }
}
