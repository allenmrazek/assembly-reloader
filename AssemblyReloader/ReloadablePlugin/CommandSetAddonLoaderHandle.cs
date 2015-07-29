using System;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin
{
    public class CommandSetAddonLoaderHandle : Command
    {
        private readonly IReloadableAddonLoader _addonLoader;
        private readonly ILoadedAssemblyHandle _handle;

        public CommandSetAddonLoaderHandle(IReloadableAddonLoader addonLoader, ILoadedAssemblyHandle handle)
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (handle == null) throw new ArgumentNullException("handle");

            _addonLoader = addonLoader;
            _handle = handle;
        }


        public override void Execute()
        {
            _addonLoader.Handle = Maybe<ILoadedAssemblyHandle>.With(_handle);
        }
    }
}
