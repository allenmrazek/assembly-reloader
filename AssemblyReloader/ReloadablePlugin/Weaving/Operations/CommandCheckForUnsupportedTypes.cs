extern alias KSP;
extern alias Cecil96;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Gui;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCheckForUnsupportedTypes : Command
    {
        private readonly IPluginInfo _pluginInfo;
        private readonly AssemblyDefinition _context;
        private readonly IGetTypeIsUnsupported _typeIsNotSupported;
        private readonly IGetAllTypesInAssemblyDefinition _typesInAssembly;
        private readonly SignalPluginCannotBeLoaded _failSignal;
        private readonly ILog _log;

        public CommandCheckForUnsupportedTypes(
            IPluginInfo pluginInfo,
            AssemblyDefinition context, 
            IGetTypeIsUnsupported typeIsNotSupported,
            IGetAllTypesInAssemblyDefinition typesInAssembly,
            SignalPluginCannotBeLoaded failSignal,
            ILog log)
        {
            if (pluginInfo == null) throw new ArgumentNullException("pluginInfo");
            if (context == null) throw new ArgumentNullException("context");
            if (typeIsNotSupported == null) throw new ArgumentNullException("typeIsNotSupported");
            if (typesInAssembly == null) throw new ArgumentNullException("typesInAssembly");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (log == null) throw new ArgumentNullException("log");

            _pluginInfo = pluginInfo;
            _context = context;
            _typeIsNotSupported = typeIsNotSupported;
            _typesInAssembly = typesInAssembly;
            _failSignal = failSignal;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Checking " + _pluginInfo.Name + " for unsupported types...");

            if (DefinitionContainsUnsupportedTypes())
            {
                _log.Error(_pluginInfo.Name + " contains unsupported types! Cannot load this plugin.");

                foreach (var unsupported in GetUnsupportedTypes())
                    _log.Error("Unsupported: " + unsupported);

                Fail();
                _failSignal.Dispatch(_pluginInfo.Name + " contains one or more unsupported types. See the log for details.");
            }

            _log.Verbose("Unsupported type check complete");
        }


        private bool DefinitionContainsUnsupportedTypes()
        {
            return _typesInAssembly.Get(_context).Any(_typeIsNotSupported.Get);
        }


        private IEnumerable<string> GetUnsupportedTypes()
        {
            return _typesInAssembly.Get(_context)
                .Where(_typeIsNotSupported.Get)
                .Select(td => td.FullName);
        }
    }
}
