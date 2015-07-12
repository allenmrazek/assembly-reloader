using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Commands
{
    public class CommandLoadAddons : Command
    {
        [Inject] public IAddonLoader AddonLoader { get; set; }
        [Inject] public ILoadedAssemblyHandle LoadedAssembly { get; set; }

        public override void Execute()
        {
            // todo: load addons 
        }
    }
}
