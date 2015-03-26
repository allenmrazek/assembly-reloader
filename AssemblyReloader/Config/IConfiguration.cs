using UnityEngine;

namespace AssemblyReloader.Config
{
    public interface IConfiguration
    {
        #region loader settings

        /// <summary>
        /// Should Addons which match the current scene (plus those marked "instant" if config option
        /// is enabled) be started immediately when a plugin is reloaded?
        /// </summary>
        bool StartAddonsForCurrentScene { get; }


        /// <summary>
        /// Reload PartModules on-the-fly, immediately (during flight & editor)
        /// </summary>
        bool ReloadPartModulesImmediately { get; }


        #endregion


        #region miscellaneous settings

        /// <summary>
        /// If enabled, "instant" KSPaddons will match all scenes. Otherwise instant addons
        /// would only be run as the game starts up and not afterwards.
        /// </summary>
        bool IgnoreCurrentSceneForInstantAddons { get; }



        #endregion


        #region il settings


        /// <summary>
        /// Calls to Assembly.CodeBase and Assembly.Location will be rewritten with a method that
        /// will return correct results, as though the assembly were loaded from disk and not memory
        /// </summary>
        bool RewriteAssemblyLocationCalls { get; }


        #endregion


        }
}
