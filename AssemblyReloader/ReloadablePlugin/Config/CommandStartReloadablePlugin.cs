using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Config
{
    public class CommandStartReloadablePlugin : Command
    {
        [Inject]
        public SignalLoadReloadablePlugin LoadPlugin { get; set; }

        [Inject]
        public IFile Plugin { get; set; }

        [Inject]
        public ILog Log { get; set; }

        // load plugin for the first time
        public override void Execute()
        {
            Log.Normal("First-load for plugin {0}", Plugin.Url);
            LoadPlugin.Dispatch();
        }
    }
}
