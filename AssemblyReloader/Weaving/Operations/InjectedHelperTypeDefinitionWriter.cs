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


        //private void WriteCodeBaseMethod(TypeDefinition typeDefinition)
        //{
        //    if (typeDefinition == null) throw new ArgumentNullException("typeDefinition");

        //    var checkMethod = new MethodDefinition(CodeBaseGetter,
        //            MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.Static,
        //            typeDefinition.Module.TypeSystem.String);

        //    typeDefinition.Methods.Add(checkMethod);

        //    var processor = checkMethod.Body.GetILProcessor();

        //    checkMethod.Parameters.Add(new ParameterDefinition("assembly", ParameterAttributes.None,
        //       checkMethod.Module.Import(typeof(Assembly))));


        //    var p = checkMethod.Body.GetILProcessor();

        //    if (p == null) _log.Warning("ILProcessor is null");
        //    p.Append(processor.Create(OpCodes.Ldarg_0));
        //    p.Append(processor.Create(OpCodes.Ldstr, "Return value"));
        //    p.Append(processor.Create(OpCodes.Ret));
        //}
    }
}
