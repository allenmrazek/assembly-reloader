using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.ReloadablePlugin.Loaders.VesselModules;
using AssemblyReloader.ReloadablePlugin.Weaving;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class PluginConfiguration : 
        IAddonSettings, 
        IPartModuleSettings, 
        IScenarioModuleSettings, 
        IVesselModuleSettings, 
        IWeaverSettings
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        // ReSharper disable UnusedMember.Global

        #region KSPAddon

        public bool InstantAppliesToEveryScene { get; set; }
        public bool StartAddonsForCurrentScene { get; set; }

        #endregion


        #region PartModule

        public bool SaveAndReloadPartModuleConfigNodes { get; set; }
        public bool CreatePartModulesImmediately { get; set; }
        public bool ResetPartModuleActions { get; set; }
        public bool ResetPartModuleEvents { get; set; }
        

        #endregion


        #region ScenarioModule

        public bool CreateScenarioModulesImmediately { get; set; }
        public bool SaveScenarioModulesBeforeDestruction { get; set; }

        #endregion


        #region VesselModule

        public bool CreateVesselModulesImmediately { get; set; }

        #endregion

        #region Intermediate Language

        public bool WritePatchedAssemblyDataToDisk { get; set; }
        public bool InterceptGameEvents { get; set; }
        public bool DontInlineFunctionsThatCallGameEvents { get; set; }

        #endregion


        public PluginConfiguration()
        {
            // set defaults
            InstantAppliesToEveryScene = true;
            StartAddonsForCurrentScene = true;

            SaveAndReloadPartModuleConfigNodes = true;
            CreatePartModulesImmediately = true;
            ResetPartModuleActions = true;
            ResetPartModuleEvents = true;

            CreateVesselModulesImmediately = true;

            CreateScenarioModulesImmediately = true;
            SaveScenarioModulesBeforeDestruction = true;

            WritePatchedAssemblyDataToDisk = true;
            InterceptGameEvents = true;
            DontInlineFunctionsThatCallGameEvents = true;
        }
    }
}
