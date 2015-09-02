extern alias Cecil96;
using System;
using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using strange.extensions.injector;
using ILProcessor = Cecil96::Mono.Cecil.Cil.ILProcessor;
using OpCodes = Cecil96::Mono.Cecil.Cil.OpCodes;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandRewriteAssemblyLocationCalls : ReturnSpecialResultIfExecutingAssembly
    {
        private readonly AssemblyLocation _assemblyLocation;


        public CommandRewriteAssemblyLocationCalls(
            AssemblyLocation assemblyLocation,
            [Name(MethodKeys.AssemblyLocation)] MethodInfo targetMethod,
            [Name(MethodKeys.AssemblyLocation)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetTypeDefinitions typeDefinitionQuery)
            : base(targetMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
        {
            if (assemblyLocation == null) throw new ArgumentNullException("assemblyLocation");

            _assemblyLocation = assemblyLocation;
        }


        protected override void LoadReturnValueOntoStack(ILProcessor processor)
        {
            processor.Emit(OpCodes.Ldstr, _assemblyLocation.Value);
        }
    }
}
