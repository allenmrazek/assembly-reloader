using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.Config
{
    class StartCommand : Command
    {
        [Inject] public ILog Log { set; get; }

        public override void Execute()
        {
            Log.Verbose("AssemblyReloader starting");

        }
    }
}
