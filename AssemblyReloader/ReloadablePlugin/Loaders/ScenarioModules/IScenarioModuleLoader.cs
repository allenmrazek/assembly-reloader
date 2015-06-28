using System;

namespace AssemblyReloader.Loaders.ScenarioModuleLoader
{
    public interface IScenarioModuleLoader
    {
        void Load(Type type);
    }
}
