using System;
using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil.Cil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
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


    //public class CommandRewriteAssemblyCodeBaseCalls : CommandRewriteAssemblyCall
    //{
    //    private readonly AssemblyCodeBase _assemblyCodeBase;

    //    public CommandRewriteAssemblyCodeBaseCalls(
    //        [Name(Keys.InterceptedMethods.CodeBase)] MethodInfo targetAssemblyMethod,
    //        [Name(Keys.InterceptedMethods.CodeBase)] IGetInstructionsInMethod callsToInterceptedMethodQuery,
    //        IGetTypeDefinitions typeDefinitionQuery,
    //        AssemblyCodeBase assemblyCodeBase)
    //        : base(targetAssemblyMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
    //    {
    //        if (assemblyCodeBase == null) throw new ArgumentNullException("assemblyCodeBase");

    //        _assemblyCodeBase = assemblyCodeBase;
    //    }

    //    protected override string GetReturnValueToInsert()
    //    {
    //        return _assemblyCodeBase.Value;
    //    }
    //}
}
