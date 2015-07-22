using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using Mono.Cecil;
using Mono.Cecil.Cil;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public class CommandRewriteAssemblyCodeBaseCalls : Command
    {
        private readonly MethodInfo _targetMethod;
        private readonly IGetInstructionsInMethod _callsToInterceptedMethodQuery;
        private readonly IGetMethodDefinitions _methodDefinitionQuery;
        private readonly IGetTypeDefinitions _typeDefinitionQuery;
        private readonly ILog _log;

        [Inject]
        public AssemblyDefinition Context { get; set; }

        [Inject]
        public TypeDefinition HelperDefinition { get; set; }


        public CommandRewriteAssemblyCodeBaseCalls(
            [Name(Keys.InterceptedMethods.CodeBase)] MethodInfo targetMethod,
            [Name(Keys.InterceptedMethods.CodeBase)] IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetMethodDefinitions methodDefinitionQuery, 
            IGetTypeDefinitions typeDefinitionQuery, 
            ILog log)
        {
            if (targetMethod == null) throw new ArgumentNullException("targetMethod");
            if (callsToInterceptedMethodQuery == null) throw new ArgumentNullException("callsToInterceptedMethodQuery");
            if (methodDefinitionQuery == null) throw new ArgumentNullException("methodDefinitionQuery");
            if (typeDefinitionQuery == null) throw new ArgumentNullException("typeDefinitionQuery");
            if (log == null) throw new ArgumentNullException("log");

            _targetMethod = targetMethod;
            _callsToInterceptedMethodQuery = callsToInterceptedMethodQuery;
            _methodDefinitionQuery = methodDefinitionQuery;
            _typeDefinitionQuery = typeDefinitionQuery;
            _log = log;
        }


        public override void Execute()
        { 
            var proxyMethod = CreateProxyMethod();

            foreach (var method in GetMethodsContainingTargetInstructions())
            {
                var calls = _callsToInterceptedMethodQuery.Get(method).ToArray();

                for (int i = 0; i < calls.Length; ++i)
                    ReplaceOriginalCallWithProxy(method, calls[i], proxyMethod);
            }
        }


        private IEnumerable<MethodDefinition> GetMethodsContainingTargetInstructions()
        {
            return _typeDefinitionQuery.Get(Context)
                .SelectMany(
                    td => _methodDefinitionQuery.Get(td)
                        .Where(md => _callsToInterceptedMethodQuery.Get(md).Any()));
        }


        private MethodDefinition CreateProxyMethod()
        {
            _log.Verbose("Creating proxy method for " + _targetMethod.Name);

            var proxyMethod = new MethodDefinition(_targetMethod.Name,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static,
                HelperDefinition.Module.Import(typeof(string)));

            proxyMethod.Parameters.Add(
                new ParameterDefinition("assembly", ParameterAttributes.None,
                    HelperDefinition.Module.Import(typeof(Assembly))));

            var getExecutingAssembly = HelperDefinition.Module.Import(typeof(Assembly).GetMethod("GetExecutingAssembly", BindingFlags.Public | BindingFlags.Static));
            var refRequals =
                HelperDefinition.Module.Import(typeof(System.Object).GetMethod("ReferenceEquals",
                    BindingFlags.Public | BindingFlags.Static));


            if (getExecutingAssembly.IsNull()) throw new Exception("Failed to find Assembly.GetExecutingAssembly");
            if (refRequals.IsNull())
                throw new Exception("Failed to find System.Object.ReferenceEquals in module type system");

            var importedRealCall = HelperDefinition.Module.Import(_targetMethod);

            var processor = proxyMethod.Body.GetILProcessor();
            var realCallOpCode = _targetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call;


            var jumpLocation = processor.Create(OpCodes.Ldarg_0);

            processor.Emit(OpCodes.Call, getExecutingAssembly);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Call, refRequals);
            processor.Emit(OpCodes.Brfalse_S, jumpLocation);
            processor.Emit(OpCodes.Ldstr, "This is a test value");
            processor.Emit(OpCodes.Ret);
            processor.Append(jumpLocation);
            processor.Emit(realCallOpCode, importedRealCall);
            processor.Emit(OpCodes.Ret);

            HelperDefinition.Methods.Add(proxyMethod);

            _log.Debug("Proxy method " + proxyMethod.FullName + " created");
            return proxyMethod;
        }


        private void ReplaceOriginalCallWithProxy(MethodDefinition methodContainingCall, Instruction instruction, MethodDefinition proxyMethod)
        {
            _log.Debug("Replacing call at " + instruction.Offset + " in " + methodContainingCall.FullName + " with " +
                       proxyMethod.FullName);

            var processor = methodContainingCall.Body.GetILProcessor();

            processor.Replace(instruction, Instruction.Create(OpCodes.Call, methodContainingCall.Module.Import(proxyMethod)));
        }
    }
}
