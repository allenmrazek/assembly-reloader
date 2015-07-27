using System;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class CommandCreateAddonsForScene : Command
    {
        private readonly IReloadableAddonLoader _addonLoader;
        private readonly IGetCurrentGameScene _getCurrentScene;
        private readonly ILog _log;

        public CommandCreateAddonsForScene(
            IReloadableAddonLoader addonLoader,
            IGetCurrentGameScene getCurrentScene, 
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
            _log.Verbose("Creating addons for current scene");
            _addonLoader.CreateAddons(_getCurrentScene.Get());
        }


    }
}
