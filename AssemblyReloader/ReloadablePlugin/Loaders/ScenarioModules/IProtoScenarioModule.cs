using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IProtoScenarioModule
    {
        Maybe<ScenarioModule> moduleRef { get; set; }
        string moduleName { get; }
        GameScenes[] TargetScenes { get; set; }

        Maybe<ScenarioModule> Load();
        ConfigNode GetData();
    }
}
