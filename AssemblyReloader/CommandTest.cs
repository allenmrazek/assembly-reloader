using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader
{
    class CommandTest : Command
    {
        [Inject]
        public ILog Log { get; set; }

        public override void Execute()
        {
            Log.Warning("CommandTest.Execute");
        }
    }
}
