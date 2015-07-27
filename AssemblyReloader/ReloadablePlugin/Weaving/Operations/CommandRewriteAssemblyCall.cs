using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Config.Keys;
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
    public abstract class CommandRewriteAssemblyCall : Command
    {
        private readonly MethodInfo _targetAssemblyMethod;
        private readonly IGetInstructionsInMethod _callsToInterceptedMethodQuery;
        private readonly IGetTypeDefinitions _typeDefinitionQuery;


        [Inject] public AssemblyDefinition Context { get; set; }
        [Inject] public TypeDefinition HelperDefinition { get; set; }
        [Inject] public ILog Log { get; set; }


        public CommandRewriteAssemblyCall(
            MethodInfo targetAssemblyMethod,
            IGetInstructionsInMethod callsToInterceptedMethodQuery,
            IGetTypeDefinitions typeDefinitionQuery)
        {
            if (targetAssemblyMethod == null) throw new ArgumentNullException("targetAssemblyMethod");
            if (callsToInterceptedMethodQuery == null) throw new ArgumentNullException("callsToInterceptedMethodQuery");
            if (typeDefinitionQuery == null) throw new ArgumentNullException("typeDefinitionQuery");

            _targetAssemblyMethod = targetAssemblyMethod;
            _callsToInterceptedMethodQuery = callsToInterceptedMethodQuery;
            _typeDefinitionQuery = typeDefinitionQuery;
        }


        public override void Execute()
        {
            var proxyMethod = CreateProxyMethod();

            foreach (var method in GetMethodsContainingTargetInstructions())
            {
                var calls = _callsToInterceptedMethodQuery.Get(method).ToArray();

// don't use foreach: collection is going to be modified
// ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < calls.Length; ++i)
                    ReplaceOriginalCallWithProxy(method, calls[i], proxyMethod);
            }
        }


        private IEnumerable<MethodDefinition> GetMethodsContainingTargetInstructions()
        {
            return _typeDefinitionQuery.Get(Context)
                .SelectMany(
                    td => td.Methods
                        .Where(md => _callsToInterceptedMethodQuery.Get(md).Any()));
        }


        protected virtual void ReplaceOriginalCallWithProxy(
            MethodDefinition inMethod, 
            Instruction callInstruction,
            MethodDefinition proxyMethod)
        {
            Log.Verbose("Replacing call at " + callInstruction.Offset + " in " + inMethod.FullName + " with " +
                       proxyMethod.FullName);

            var processor = inMethod.Body.GetILProcessor();

            processor.Replace(callInstruction, Instruction.Create(OpCodes.Call, inMethod.Module.Import(proxyMethod)));
        }


        protected virtual MethodDefinition CreateProxyMethod()
        {
            Log.Verbose("Creating proxy method for " + _targetAssemblyMethod.Name);

            var proxyMethod = new MethodDefinition(_targetAssemblyMethod.Name,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static,
                HelperDefinition.Module.Import(_targetAssemblyMethod.ReturnType));

            proxyMethod.Parameters.Add(
                new ParameterDefinition("assembly", ParameterAttributes.None,
                    HelperDefinition.Module.Import(typeof(Assembly))));

            var getExecutingAssembly = HelperDefinition.Module.Import(typeof(Assembly).GetMethod("GetExecutingAssembly", BindingFlags.Public | BindingFlags.Static));
            var refRequals =
                HelperDefinition.Module.Import(typeof(System.Object).GetMethod("ReferenceEquals",
                    BindingFlags.Public | BindingFlags.Static));
            var importedRealCall = HelperDefinition.Module.Import(_targetAssemblyMethod);

            if (getExecutingAssembly.IsNull()) throw new Exception("Failed to find Assembly.GetExecutingAssembly");

            if (refRequals.IsNull())
                throw new Exception("Failed to find System.Object.ReferenceEquals in module type system");

            if (importedRealCall.IsNull())
                throw new Exception("Failed to import target method " + _targetAssemblyMethod.Name);


            CreateProxyMethodBody(proxyMethod.Body.GetILProcessor(), importedRealCall,
                getExecutingAssembly, refRequals);

            HelperDefinition.Methods.Add(proxyMethod);

            Log.Verbose("Proxy method " + proxyMethod.FullName + " created");
            return proxyMethod;
        }


        protected virtual void CreateProxyMethodBody(
            ILProcessor processor,
            MethodReference assemblyCodeBaseCall,
            MethodReference assemblyGetExecutingAssemblyCall,
            MethodReference objectReferenceEqualsCall)
        {
            var jumpLocation = processor.Create(OpCodes.Ldarg_0);

            processor.Emit(OpCodes.Call, assemblyGetExecutingAssemblyCall);
            processor.Emit(OpCodes.Ldarg_0);
            processor.Emit(OpCodes.Call, objectReferenceEqualsCall);
            processor.Emit(OpCodes.Brfalse_S, jumpLocation);
            processor.Emit(OpCodes.Ldstr, GetReturnValueToInsert());
            processor.Emit(OpCodes.Ret);
            processor.Append(jumpLocation);
            processor.Emit(OpCodes.Callvirt, assemblyCodeBaseCall);
            processor.Emit(OpCodes.Ret);
        }


        protected abstract string GetReturnValueToInsert();
    }
}
