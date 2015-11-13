extern alias KSP;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using UnityEngine;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class CommandCreateAddonsForScene : Command
    {
        private readonly IReloadableAddonLoader _addonLoader;
        private readonly IGetCurrentStartupScene _getCurrentScene;
        private readonly ILog _log;

// ReSharper disable once MemberCanBeProtected.Global
        public CommandCreateAddonsForScene(
            IReloadableAddonLoader addonLoader,
            IGetCurrentStartupScene getCurrentScene, 
            ILog log)
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (getCurrentScene == null) throw new ArgumentNullException("getCurrentScene");
            if (log == null) throw new ArgumentNullException("log");

            _addonLoader = addonLoader;
            _getCurrentScene = getCurrentScene;
            _log = log;
        }


        public override void Execute()
        {
            CreateAddons(_getCurrentScene.Get());
        }


        protected void CreateAddons(KSPAddon.Startup scene)
        {
            _log.Verbose("Creating addons for current scene: " + _getCurrentScene.Get());
            _addonLoader.CreateAddons(_getCurrentScene.Get());
        }


        protected IReloadableAddonLoader AddonLoader
        {
            get { return _addonLoader; }
        }
    }
}
