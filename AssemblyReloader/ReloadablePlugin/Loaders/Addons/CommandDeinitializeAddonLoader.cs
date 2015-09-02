using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class CommandDeinitializeAddonLoader : Command
    {
        private readonly ILog _log;
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IReloadableAddonLoader _addonLoader;
        private readonly IReloadableAddonUnloader _addonUnloader;

        public CommandDeinitializeAddonLoader(
            ILoadedAssemblyHandle handle,
            IReloadableAddonLoader addonLoader,
            IReloadableAddonUnloader addonUnloader,
            ILog log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (addonUnloader == null) throw new ArgumentNullException("addonUnloader");
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _addonLoader = addonLoader;
            _addonUnloader = addonUnloader;
            _log = log;
        }


        public override void Execute()
        {
            if (_addonLoader.Handle.Any() && _addonLoader.Handle.Single() == _handle)
            {
                int liveAddonsDestroyed = _addonUnloader.DestroyAddons(_addonLoader.Handle.Single());
                _log.Verbose(string.Format("Destroyed {0} live addons", liveAddonsDestroyed));
            }

            _addonLoader.Handle = Maybe<ILoadedAssemblyHandle>.None;
        }
    }
}
