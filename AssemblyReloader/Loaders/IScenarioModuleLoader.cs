using System;

namespace AssemblyReloader.Loaders
{
    public interface IScenarioModuleLoader
    {
        void Load(Type type);
    }
}
