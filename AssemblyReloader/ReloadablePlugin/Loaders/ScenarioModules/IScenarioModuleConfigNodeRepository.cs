using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleConfigNodeRepository
    {
        void Store(TypeIdentifier identifier, ConfigNode config);
        Maybe<ConfigNode> Retrieve(TypeIdentifier smType);
        void Clear();
        int Count();
    }
}
