using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleFactory
    {
        void Create(IProtoScenarioModule psm, Type type);
    }
}
