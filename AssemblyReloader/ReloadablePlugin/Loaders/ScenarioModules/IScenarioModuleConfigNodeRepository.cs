using System;
using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleConfigNodeRepository
    {
        void Store(ITypeIdentifier identifier, ConfigNode config);
        Maybe<ConfigNode> Retrieve(Type smType);
    }
}
