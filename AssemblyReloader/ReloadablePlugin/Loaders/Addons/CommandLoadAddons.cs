using System;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once UnusedMember.Global
    public class CommandLoadAddons : Command
    {
        [Inject] public IAddonLoader AddonLoader { get; set; }
        [Inject] public ILoadedAssemblyHandle LoadedAssembly { get; set; }
        [Inject] public ILog Log { get; set; }

        public override void Execute()
        {
            try
            {
                Log.Debug("Loading addons");
                //AddonLoader.CreateAddons(LoadedAssembly);
            }
            catch (Exception e)
            {
                Log.Error("Exception while loading addon types: " + e);
                throw; // todo: maybe a special error message or log?
            }
        }
    }
}
