using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class CommandLoadPartModuleConfigNode : Command
    {
        private readonly PartModule _partModule;
        private readonly PartModuleDescriptor _descriptor;
        private readonly ILog _log;


        public CommandLoadPartModuleConfigNode(
            PartModule partModule, 
            PartModuleDescriptor descriptor,
            [Name(LogKeys.PartModuleLoader)] ILog log)
        {
            if (partModule == null) throw new ArgumentNullException("partModule");
            if (descriptor == null) throw new ArgumentNullException("descriptor");
            if (log == null) throw new ArgumentNullException("log");

            _partModule = partModule;
            _descriptor = descriptor;
            _log = log;
        }


        public override void Execute()
        {
            _log.Normal("todo: load ConfigNode for " + _partModule.GetType().FullName);

        }
    }
}
