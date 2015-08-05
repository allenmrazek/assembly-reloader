using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class PluginConfiguration : IAddonSettings, IPartModuleSettings
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        // ReSharper disable UnusedMember.Global

        #region KSPAddon

        public bool InstantlyAppliesToAllScenes { get; set; }
        public bool StartAddonsForCurrentScene { get; set; }

        #endregion


        #region PartModule

        public bool SaveAndReloadPartModuleConfigNodes { get; set; }
        public bool ReplacePartModulesInstancesImmediately { get; set; }
        public bool ResetPartModuleActions { get; set; }
        public bool ResetPartModuleEvents { get; set; }
        

        #endregion


        #region ScenarioModule

        #endregion


        #region Intermediate Language

        #endregion


        public PluginConfiguration()
        {
            // set defaults
            InstantlyAppliesToAllScenes = true;
            StartAddonsForCurrentScene = true;

            SaveAndReloadPartModuleConfigNodes = true;
            ReplacePartModulesInstancesImmediately = true;
            ResetPartModuleActions = true;
            ResetPartModuleEvents = true;
        }
    }
}
