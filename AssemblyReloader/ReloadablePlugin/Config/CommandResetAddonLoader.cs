using System;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class CommandResetAddonLoader : Command
    {
        private readonly IReloadableAddonLoader _addonLoader;

        public CommandResetAddonLoader(IReloadableAddonLoader addonLoader)
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");

            _addonLoader = addonLoader;
        }


        public override void Execute()
        {
            // todo: destroy existing addons
            _addonLoader.Handle = Maybe<ILoadedAssemblyHandle>.None;
        }
    }
}
