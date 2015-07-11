using ReeperCommon.Serialization;

namespace AssemblyReloader.Config
{
    public class PluginConfiguration
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        // ReSharper disable UnusedMember.Global

        #region KSPAddon


        [ReeperPersistent] public Setting<bool> StartAddonsForCurrentScene = true;
        [ReeperPersistent] public Setting<bool> InstantlyAppliesToEveryScene = true;

        #endregion


        #region PartModule

        [ReeperPersistent] public Setting<bool> ReloadPartModulesImmediately = true;
        [ReeperPersistent] public Setting<bool> ReusePartModuleConfigsFromPrevious = true;

        #endregion


        #region ScenarioModule

        [ReeperPersistent] public Setting<bool> ReloadScenarioModulesImmediately = true;
        [ReeperPersistent] public Setting<bool> SaveScenarioModuleConfigBeforeReloading = true;

        #endregion


        #region Intermediate Language

        [ReeperPersistent] public Setting<bool> InjectHelperType = true;
        [ReeperPersistent] public Setting<bool> RewriteAssemblyLocationCalls = true;
        [ReeperPersistent] public Setting<bool> WriteReweavedAssemblyToDisk = true;

        #endregion

    }
}
