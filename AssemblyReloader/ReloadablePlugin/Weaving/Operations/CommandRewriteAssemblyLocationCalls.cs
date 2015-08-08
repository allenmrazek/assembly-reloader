using System;
using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil.Cil;

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


// ReSharper disable once ClassNeverInstantiated.Global
    //public class CommandRewriteAssemblyLocationCalls : CommandRewriteAssemblyCall
    //{
    //    private readonly AssemblyLocation _assemblyLocation;

    //    public CommandRewriteAssemblyLocationCalls(
    //        [Name(Keys.InterceptedMethods.Location)] MethodInfo targetAssemblyMethod, 
    //        [Name(Keys.InterceptedMethods.Location)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
    //        IGetTypeDefinitions typeDefinitionQuery,
    //        AssemblyLocation assemblyLocation) : base(targetAssemblyMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
    //    {
    //        if (assemblyLocation == null) throw new ArgumentNullException("assemblyLocation");

    //        _assemblyLocation = assemblyLocation;
    //    }

    //    protected override string GetReturnValueToInsert()
    //    {
    //        return _assemblyLocation.Value;
    //    }
    //}
}
