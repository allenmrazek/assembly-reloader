namespace AssemblyReloader.Config
{
    public class Configuration : IConfiguration
    {
        public bool ReplacePartModulesInFlight { get; private set; }
        public bool ReloadPartModuleConfigsForPartModulesInFlight { get; private set; }
        public bool ReplaceInternalModulesInFlight { get; private set; }
        public bool ReloadInternalModuleConfigsInFlight { get; private set; }
        public bool RestartScenarioModulesForCurrentScene { get; private set; }
        public bool ReloadScenarioModulePersistentConfigs { get; private set; }
        public bool StartAddonsForCurrentScene { get; private set; }
        public bool IgnoreCurrentSceneForInstantAddons { get; private set; }
    }
}
