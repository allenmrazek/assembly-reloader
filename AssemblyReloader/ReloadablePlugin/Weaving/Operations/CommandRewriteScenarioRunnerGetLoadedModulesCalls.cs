using System.Reflection;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations.Keys;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandRewriteScenarioRunnerGetLoadedModulesCalls : RewriteCall
    {
        private const string ProxyMethodName = "Proxy_GetLoadedModules";


        public CommandRewriteScenarioRunnerGetLoadedModulesCalls(
            [Name(MethodKeys.ScenarioRunnerGetLoadedModules)] MethodInfo targetMethod,
            [Name(MethodKeys.ScenarioRunnerGetLoadedModules)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetTypeDefinitions typeDefinitionQuery) : base(targetMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
        {
        }


        protected override void ReplaceOriginalCallWithProxy(MethodDefinition inMethod, Instruction callInstruction)
        {
            inMethod.Body.GetILProcessor().Replace(callInstruction,
                Instruction.Create(OpCodes.Call,
                    inMethod.Module.Import(typeof (Proxies.ScenarioRunnerProxyMethods).GetMethod(ProxyMethodName,
                        BindingFlags.Public | BindingFlags.Static))));
        }
    }
}
