using System;
using AssemblyReloader.Commands;
using Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old.Definition.Operations.old
{
    public class InjectedHelperTypeDefinitionWriter : WeaveOperation
    {
        private readonly ICommand<TypeDefinition> _writeMethods;

        public const string Namespace = "ART.Injected";
        public const string TypeName = "AssemblyLocationHelper";

        public InjectedHelperTypeDefinitionWriter(
            ICommand<TypeDefinition> writeMethods)
        {
            if (writeMethods == null) throw new ArgumentNullException("writeMethods");

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
