extern alias KSP;
using System;
using System.Collections.Generic;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class CommandCreateAddonsForScene : Command
    {
        private readonly IReloadableAddonLoader _addonLoader;
        private readonly IGetCurrentStartupScene _getCurrentScene;
        private readonly IAddonSettings _addonSettings;
        private readonly ILog _log;

// ReSharper disable once MemberCanBeProtected.Global
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

            _addonLoader = addonLoader;
            _getCurrentScene = getCurrentScene;
            _addonSettings = addonSettings;
            _log = log;
        }


        public override void Execute()
        {
            if (!_addonSettings.StartAddonsForCurrentScene) return;

            _log.Verbose("Creating addons for current scene");
            _addonLoader.CreateAddons(_getCurrentScene.Get());
        }


        protected IReloadableAddonLoader AddonLoader
        {
            get { return _addonLoader; }
        }
    }
}
