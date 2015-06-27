using System;

namespace AssemblyReloader.Loaders.ScenarioModuleLoader
{
    public interface IScenarioModuleUnloader
    {
        void Unload(Type type);
    }
}
