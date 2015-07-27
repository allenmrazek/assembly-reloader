using System;
using System.Reflection;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandRewriteAssemblyCodeBaseCalls : CommandRewriteAssemblyCall
    {
        private readonly AssemblyCodeBase _assemblyCodeBase;

        public CommandRewriteAssemblyCodeBaseCalls(
            [Name(Keys.InterceptedMethods.CodeBase)] MethodInfo targetAssemblyMethod,
            [Name(Keys.InterceptedMethods.CodeBase)] IGetInstructionsInMethod callsToInterceptedMethodQuery,
            IGetTypeDefinitions typeDefinitionQuery,
            AssemblyCodeBase assemblyCodeBase)
            : base(targetAssemblyMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
        {
            if (assemblyCodeBase == null) throw new ArgumentNullException("assemblyCodeBase");

            _assemblyCodeBase = assemblyCodeBase;
        }

        protected override string GetReturnValueToInsert()
        {
            return _assemblyCodeBase.Value;
        }
    }
}
