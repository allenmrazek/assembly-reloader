using System;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.StrangeIoC.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandDispatchLoadersFinished : Command
    {
        private readonly SignalLoadersFinished _loadersFinishedSignal;


        public CommandDispatchLoadersFinished(SignalLoadersFinished loadersFinishedSignal)
        {
            if (loadersFinishedSignal == null) throw new ArgumentNullException("loadersFinishedSignal");
            _loadersFinishedSignal = loadersFinishedSignal;
        }


        public override void Execute()
        {
            _loadersFinishedSignal.Dispatch();
        }
    }
}
