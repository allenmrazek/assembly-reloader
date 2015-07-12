using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
    public class CommandLoadPluginAssembly : Command
    {
        [Inject] public ILog Log { get; set; }
        [Inject] public SignalAssemblyWasLoaded LoadedSignal { get; set; }


        public override void Execute()
        {
            Log.Warning("CommandLoadPluginAssembly.Execute");

        }
    }
}
