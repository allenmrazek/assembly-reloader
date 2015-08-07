using System;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleConfigNodeRepository
    {
        void Store(Type smType, ConfigNode config);
        Maybe<ConfigNode> Retrieve(Type smType);
    }
}
