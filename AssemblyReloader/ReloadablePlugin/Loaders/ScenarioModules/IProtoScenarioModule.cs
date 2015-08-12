extern alias KSP;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IProtoScenarioModule
    {
        Maybe<KSP::ScenarioModule> moduleRef { get; set; }
        string moduleName { get; }
        KSP::GameScenes[] TargetScenes { get; set; }

        Maybe<KSP::ScenarioModule> Load();
        KSP::ConfigNode GetData();
    }
}
