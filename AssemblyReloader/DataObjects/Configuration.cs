using System;
using ReeperCommon.Serialization;

namespace AssemblyReloader.DataObjects
{
    public class Configuration
    {
        // KSPAddon
        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.Addon)]
        [ConfigItemDescription("Start addons for current scene immediately")]
        public bool StartAddonsForCurrentScene = true;

        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.Addon)]
        [ConfigItemDescription("KSPAddon.Instant applies to all scenes")]
        public bool InstantlyAppliesToEveryScene = true;



        // PartModule
        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.PartModule)]
        [ConfigItemDescription("Swap PartModules immediately")]
        public bool ReloadPartModulesImmediately = true;

        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.PartModule)]
        [ConfigItemDescription("New PartModule will use ConfigNode saved by previous PartModule")]
        public bool ReusePartModuleConfigsFromPrevious = true;



        
        // ScenarioModule
        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.ScenarioModule)]
        [ConfigItemDescription("Swap ScenarioModules immediately")]
        public bool ReloadScenarioModulesImmediately = true;

        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.ScenarioModule)]
        [ConfigItemDescription("New ScenarioModule will use ConfigNode saved by previous ScenarioModule")]
        public bool ReuseScenarioModuleConfigsFromPrevious = true;



        // Intermediate Language
        [ReeperPersistent, PanelCategory(PanelCategoryAttribute.CategoryType.IntermediateLanguage)]
        [ConfigItemDescription("Intercept calls to Assembly.CodeBase and Assembly.Location")]
        public bool RewriteAssemblyLocationCalls = true;
    }
}
