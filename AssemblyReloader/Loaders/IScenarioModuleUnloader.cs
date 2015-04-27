using System;

namespace AssemblyReloader.Loaders
{
    public interface IScenarioModuleUnloader
    {
        void Unload(Type type);
    }
}
