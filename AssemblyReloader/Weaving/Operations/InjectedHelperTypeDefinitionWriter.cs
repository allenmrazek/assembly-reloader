using System;
using AssemblyReloader.CompositeRoot.Commands;
using Mono.Cecil;
using ReeperCommon.Logging;
using TypeAttributes = Mono.Cecil.TypeAttributes;

namespace AssemblyReloader.Weaving.Operations
{
    public class InjectedHelperTypeDefinitionWriter : WeaveOperation
    {
        private readonly ILog _log;
        private readonly ICommand<TypeDefinition> _writeMethods;

        public const string Namespace = "ART.Injected";
        public const string TypeName = "HelperType";

        public InjectedHelperTypeDefinitionWriter(
            ILog log, 
            ICommand<TypeDefinition> writeMethods)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (writeMethods == null) throw new ArgumentNullException("writeMethods");

            _log = log;
            _writeMethods = writeMethods;
        }


        public override void Run(AssemblyDefinition definition)
        {
            var td = new TypeDefinition(
                Namespace,
                TypeName, 
                TypeAttributes.Class | TypeAttributes.BeforeFieldInit,
                definition.MainModule.Import(typeof(System.Object)));
     
            definition.MainModule.Types.Add(td);

            _writeMethods.Execute(td);
        }
    }
}
