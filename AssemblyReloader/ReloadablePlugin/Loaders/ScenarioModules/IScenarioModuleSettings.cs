namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleSettings
    {
        bool CreateScenarioModulesImmediately { get; }
        bool SaveScenarioModulesBeforeDestruction { get; }
    }
}
