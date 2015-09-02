extern alias Cecil96;
using System;
using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using strange.extensions.injector;
using ILProcessor = Cecil96::Mono.Cecil.Cil.ILProcessor;
using OpCodes = Cecil96::Mono.Cecil.Cil.OpCodes;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandRewriteAssemblyCodeBaseCalls : ReturnSpecialResultIfExecutingAssembly
    {
        private readonly AssemblyCodeBase _assemblyCodeBase;

        public CommandRewriteAssemblyCodeBaseCalls(
            AssemblyCodeBase assemblyCodeBase,
            [Name(MethodKeys.AssemblyCodeBase)] MethodInfo targetMethod, 
            [Name(MethodKeys.AssemblyCodeBase)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetTypeDefinitions typeDefinitionQuery) : base(targetMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
        {
            if (assemblyCodeBase == null) throw new ArgumentNullException("assemblyCodeBase");

            _assemblyCodeBase = assemblyCodeBase;
        }

        protected override void LoadReturnValueOntoStack(ILProcessor processor)
        {
            processor.Emit(OpCodes.Ldstr, _assemblyCodeBase.Value);
        }
    }
}
