using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin
{
    public class CommandDeinitializeAddonLoader : Command
    {
        private readonly ILog _log;
        private readonly IReloadableAddonLoader _addonLoader;
        private readonly IReloadableAddonUnloader _addonUnloader;

        public CommandDeinitializeAddonLoader(
            IReloadableAddonLoader addonLoader,
            IReloadableAddonUnloader addonUnloader,
            ILog log)
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (addonUnloader == null) throw new ArgumentNullException("addonUnloader");
            if (log == null) throw new ArgumentNullException("log");

            _addonLoader = addonLoader;
            _addonUnloader = addonUnloader;
            _log = log;
        }


        public override void Execute()
        {
            if (_addonLoader.Handle.Any())
            {
                int liveAddonsDestroyed = _addonUnloader.DestroyAddons(_addonLoader.Handle.Single());
                _log.Verbose(string.Format("Destroyed {0} live addons", liveAddonsDestroyed));
            }

            _addonLoader.Handle = Maybe<ILoadedAssemblyHandle>.None;
        }
    }
}
