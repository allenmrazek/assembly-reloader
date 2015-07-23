using System;
using System.Reflection;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandRewriteAssemblyLocationCalls : CommandRewriteAssemblyCall
    {
        private readonly AssemblyLocation _assemblyLocation;

        public CommandRewriteAssemblyLocationCalls(
            [Name(Keys.InterceptedMethods.Location)] MethodInfo targetAssemblyMethod, 
            [Name(Keys.InterceptedMethods.Location)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetMethodDefinitions methodDefinitionQuery, 
            IGetTypeDefinitions typeDefinitionQuery,
            AssemblyLocation assemblyLocation) : base(targetAssemblyMethod, callsToInterceptedMethodQuery, methodDefinitionQuery, typeDefinitionQuery)
        {
            if (assemblyLocation == null) throw new ArgumentNullException("assemblyLocation");

            _assemblyLocation = assemblyLocation;
        }

        protected override string GetReturnValueToInsert()
        {
            return _assemblyLocation.Value;
        }
    }
}
