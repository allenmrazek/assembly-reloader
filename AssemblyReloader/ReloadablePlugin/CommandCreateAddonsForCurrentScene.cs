using System;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin
{
    public class CommandCreateAddonsForScene : Command
    {
        protected readonly IReloadableAddonLoader AddonLoader;
        private readonly IGetCurrentStartupScene _getCurrentScene;
        private readonly IAddonSettings _addonSettings;
        private readonly ILog _log;

        public CommandCreateAddonsForScene(
            IReloadableAddonLoader addonLoader,
            IGetCurrentStartupScene getCurrentScene, 
            IAddonSettings addonSettings,
            ILog log)
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (getCurrentScene == null) throw new ArgumentNullException("getCurrentScene");
            if (addonSettings == null) throw new ArgumentNullException("addonSettings");
            if (log == null) throw new ArgumentNullException("log");

            AddonLoader = addonLoader;
            _getCurrentScene = getCurrentScene;
            _addonSettings = addonSettings;
            _log = log;
        }


        public override void Execute()
        {
            if (_addonSettings.InstantlyAppliesToAllScenes)
            {
                _log.Verbose("Creating instant addons");
                AddonLoader.CreateAddons(KSPAddon.Startup.Instantly);
            }


            if (_addonSettings.StartAddonsForCurrentScene)
            {
                _log.Verbose("Creating addons for current scene");
                AddonLoader.CreateAddons(_getCurrentScene.Get());
            }
        }
    }
}
