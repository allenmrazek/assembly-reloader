using ReeperCommon.Serialization;

namespace AssemblyReloader.DataObjects
{
    public class PluginConfiguration
    {
        // ReSharper disable ConvertToConstant.Global
        // ReSharper disable FieldCanBeMadeReadOnly.Global

        // KSPAddon
        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.Addon)]
        //[ConfigItemDescription("Start addons for current scene immediately")]
        [ReeperPersistent, Persistent]
        public bool StartAddonsForCurrentScene = true;

        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.Addon)]
        //[ConfigItemDescription("KSPAddon.Instant applies to all scenes")]
        [ReeperPersistent, Persistent]
        public bool InstantlyAppliesToEveryScene = true;




        // PartModule
        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.PartModule)]
        //[ConfigItemDescription("Swap PartModules immediately")]
        [ReeperPersistent]
        public bool ReloadPartModulesImmediately = true;

        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.PartModule)]
        //[ConfigItemDescription("New PartModule will use ConfigNode saved by previous PartModule")]
        [ReeperPersistent, Persistent]
        public bool ReusePartModuleConfigsFromPrevious = true;



        
        // ScenarioModule
        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.ScenarioModule)]
        //[ConfigItemDescription("Swap ScenarioModules immediately")]
        [ReeperPersistent, Persistent]
        public bool ReloadScenarioModulesImmediately = true;

        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.ScenarioModule)]
        //[ConfigItemDescription("Serialize ScenarioModule ConfigNode before reloading")]
        [ReeperPersistent, Persistent]
        public bool SaveScenarioModuleConfigBeforeReloading = true;



        // Intermediate Language
        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.IntermediateLanguage)] 
        //[ConfigItemDescription("Inject helper type (required for method interception)")] 
        public bool InjectHelperType = true;

        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.IntermediateLanguage)]
        //[ConfigItemDescription("Intercept calls to Assembly.CodeBase and Assembly.Location")]
        [ReeperPersistent, Persistent]
        public bool RewriteAssemblyLocationCalls = true;


        //[ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.IntermediateLanguage)]
        //[ConfigItemDescription("Write reweaved assembly to disk")]
        [ReeperPersistent, Persistent]
        public bool WriteReweavedAssemblyToDisk = true;



    }
}
