using System;
using System.Reflection;
using AssemblyReloader.StrangeIoC.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandRewriteAssemblyLocationCalls : CommandRewriteAssemblyCall
    {
        private readonly AssemblyLocation _assemblyLocation;

        public CommandRewriteAssemblyLocationCalls(
            [Name(Keys.InterceptedMethods.Location)] MethodInfo targetAssemblyMethod, 
            [Name(Keys.InterceptedMethods.Location)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetTypeDefinitions typeDefinitionQuery,
            AssemblyLocation assemblyLocation) : base(targetAssemblyMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
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
