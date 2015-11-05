using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.Config
{
    public class CommandDebugPrint : Command
    {
        private readonly ILog _log;

        public CommandDebugPrint(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }


        public override void Execute()
        {
            _log.Warning("DebugPrint executed");
        }
    }
}
