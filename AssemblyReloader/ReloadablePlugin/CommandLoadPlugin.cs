using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Commands
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
    public class CommandLoadPlugin : Command
    {
        [Inject] public ILog Log { get; set; }
        [Inject] public SignalAssemblyLoaded LoadedSignal { get; set; }


        public override void Execute()
        {
            Log.Warning("CommandLoadPlugin.Execute");

            // now load the various types for this plugin
            LoadedSignal.Dispatch(null);
        }
    }
}
