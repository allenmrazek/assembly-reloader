using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleUnloader
    {
        void Unload(Type type);
    }
}
