using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    public class CommandRewriteAssemblyLocationCalls : Command
    {
        [Inject] public AssemblyDefinition AssemblyDefinition { get; set; }

        [Inject(LogKeys.PluginContext)]
        public ILog Log { get; set; }


        public override void Execute()
        {
            Log.Debug("Rewriting assembly location calls");
        }
    }
}
