using AssemblyReloader.StrangeIoC.extensions.command.impl;

namespace AssemblyReloader.Common
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandNull : Command
    {
        public override void Execute()
        {
            // no op
        }
    }
}
