using System;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders
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
