extern alias KSP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandCheckForUnsupportedTypes : Command
    {
        private readonly AssemblyDefinition _context;
        private readonly IGetTypeIsUnsupported _typeIsNotSupported;
        private readonly IGetAllTypesInAssemblyDefinition _typesInAssembly;
        private readonly SignalPluginCannotBeLoaded _failSignal;
        private readonly ILog _log;

        public CommandCheckForUnsupportedTypes(
            AssemblyDefinition context, 
            IGetTypeIsUnsupported typeIsNotSupported,
            IGetAllTypesInAssemblyDefinition typesInAssembly,
            SignalPluginCannotBeLoaded failSignal,
            ILog log)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (typeIsNotSupported == null) throw new ArgumentNullException("typeIsNotSupported");
            if (typesInAssembly == null) throw new ArgumentNullException("typesInAssembly");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (log == null) throw new ArgumentNullException("log");

            _context = context;
            _typeIsNotSupported = typeIsNotSupported;
            _typesInAssembly = typesInAssembly;
            _failSignal = failSignal;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Checking " + _context.FullName + " for unsupported types...");

            if (DefinitionContainsUnsupportedTypes())
            {
                _log.Error(_context.Name + " contains unsupported types! Cannot load this plugin.");

                foreach (var unsupported in GetUnsupportedTypes())
                    _log.Error("Unsupported: " + unsupported);

                Fail();
                _failSignal.Dispatch(_context.Name + " contains unsupported types");
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
        //private static bool IsUnsupportedType(TypeDefinition typeDefinition)
        //{
        //    return Unsupported.All(unsupportedType =>
        //    {
        //        return unsupportedType.IsInterface
        //    });
        //}
    }
}
