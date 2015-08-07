namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public interface IScenarioModuleSettings
    {
        bool ReloadScenarioModulesImmediately { get; }
        bool SaveScenarioModuleBeforeDestroying { get; }
    }
}
