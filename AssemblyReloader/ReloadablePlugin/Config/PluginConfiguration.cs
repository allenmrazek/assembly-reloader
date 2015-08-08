using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class PluginConfiguration : IAddonSettings, IPartModuleSettings, IScenarioModuleSettings
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        // ReSharper disable UnusedMember.Global

        #region KSPAddon

        public bool InstantlyAppliesToAllScenes { get; set; }
        public bool StartAddonsForCurrentScene { get; set; }

        #endregion


        #region PartModule

        public bool SaveAndReloadPartModuleConfigNodes { get; set; }
        public bool ReloadPartModuleInstancesImmediately { get; set; }
        public bool ResetPartModuleActions { get; set; }
        public bool ResetPartModuleEvents { get; set; }
        

        #endregion


        #region ScenarioModule

        public bool ReloadScenarioModulesImmediately { get; set; }
        public bool SaveScenarioModuleBeforeDestroying { get; set; }

        #endregion


        #region Intermediate Language

        #endregion


        public PluginConfiguration()
        {
            // set defaults
            InstantlyAppliesToAllScenes = true;
            StartAddonsForCurrentScene = true;

            SaveAndReloadPartModuleConfigNodes = true;
            ReloadPartModuleInstancesImmediately = true;
            ResetPartModuleActions = true;
            ResetPartModuleEvents = true;

            ReloadScenarioModulesImmediately = true;
            SaveScenarioModuleBeforeDestroying = true;
        }
    }
}
