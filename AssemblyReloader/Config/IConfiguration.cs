using UnityEngine;

namespace AssemblyReloader.Config
{
    public interface IConfiguration
    {
        /// <summary>
        /// Replace PartModules on instantiated parts on-the-fly when reloading a plugin
        /// </summary>
        bool ReplacePartModulesInFlight { get; }


        /// <summary>
        /// Should we use the existing (persistent) ConfigNodes for PartModules in flight when we replace them
        /// or use the default value supplied by the part config?
        /// </summary>
        bool ReloadPartModuleConfigsForPartModulesInFlight { get; }


        bool ReplaceInternalModulesInFlight { get; }
        bool ReloadInternalModuleConfigsInFlight { get; }


        bool RestartScenarioModulesForCurrentScene { get; }
        bool ReloadScenarioModulePersistentConfigs { get; }

        /// <summary>
        /// Create MonoBehaviours tagged with the KSPAddon attribute for the current scene
        /// immediately after reloading a plugin
        /// </summary>
        bool StartAddonsForCurrentScene { get; }


        /// <summary>
        /// Should KSPAddon attributes marked with "instantly" be created regardless of scene?
        /// </summary>
        bool IgnoreCurrentSceneForInstantAddons { get; }





        
    }
}
