extern alias KSP;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleConfigNodeRepository
    {
        void Store(TypeIdentifier identifier, KSP::ConfigNode config);
        Maybe<KSP::ConfigNode> Retrieve(TypeIdentifier smType);
    }
}
