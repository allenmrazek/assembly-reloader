using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleLoader
    {
        void Load(Type type);
    }
}
