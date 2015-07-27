using AssemblyReloader.ReloadablePlugin.Loaders.Addons;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class PluginConfiguration : IAddonSettings
    {
        // ReSharper disable FieldCanBeMadeReadOnly.Global
        // ReSharper disable UnusedMember.Global

        #region KSPAddon

        public bool InstantlyAppliesToAllScenes { get; set; }
        public bool StartAddonsForCurrentScene { get; set; }

        #endregion


        #region PartModule


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
        }
    }
}
