extern alias Cecil96;
using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Extensions;
using MethodReference = Cecil96::Mono.Cecil.MethodReference;
using MethodDefinition = Cecil96::Mono.Cecil.MethodDefinition;
using ParameterDefinition = Cecil96::Mono.Cecil.ParameterDefinition;
using ILProcessor = Cecil96::Mono.Cecil.Cil.ILProcessor;
using Instruction = Cecil96::Mono.Cecil.Cil.Instruction;
using OpCodes = Cecil96::Mono.Cecil.Cil.OpCodes;
using MethodAttributes = Cecil96::Mono.Cecil.MethodAttributes;
using ParameterAttributes = Cecil96::Mono.Cecil.ParameterAttributes;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public abstract class ReturnSpecialResultIfExecutingAssembly : RewriteCall
    {
        private MethodDefinition _replacementMethodDefinition;


        public ReturnSpecialResultIfExecutingAssembly(
            MethodInfo targetMethod, 
            IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetTypeDefinitions typeDefinitionQuery) : 
            base(targetMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
        {
        }


        public override void Execute()
        {
            _replacementMethodDefinition = CreateProxyMethod();
            base.Execute();
        }


        protected override void ReplaceOriginalCallWithProxy(MethodDefinition inMethod, Instruction callInstruction)
        {
            Log.Verbose("Replacing call at " + callInstruction.Offset + " in " + inMethod.FullName + " with " +
                       _replacementMethodDefinition.FullName);

            var processor = inMethod.Body.GetILProcessor();

            processor.Replace(callInstruction, Instruction.Create(OpCodes.Call, inMethod.Module.Import(_replacementMethodDefinition)));
        }




        protected virtual MethodDefinition CreateProxyMethod()
        {
            if (HelperDefinition.Methods.Any(m => m.Name == TargetMethod.Name))
                throw new ProxyMethodAlreadyDefinedException(HelperDefinition,
                    HelperDefinition.Methods.First(m => m.Name == TargetMethod.Name));

            Log.Verbose("Creating proxy method for " + TargetMethod.Name);

            var proxyMethod = new MethodDefinition(TargetMethod.Name,
                MethodAttributes.HideBySig | MethodAttributes.Public | MethodAttributes.Static,
                HelperDefinition.Module.Import(TargetMethod.ReturnType));

            proxyMethod.Parameters.Add(
                new ParameterDefinition("assembly", ParameterAttributes.None,
                    HelperDefinition.Module.Import(typeof(Assembly))));

            var getExecutingAssembly = HelperDefinition.Module.Import(typeof(Assembly).GetMethod("GetExecutingAssembly", BindingFlags.Public | BindingFlags.Static));
            var refRequals =
                HelperDefinition.Module.Import(typeof(Object).GetMethod("ReferenceEquals",
                    BindingFlags.Public | BindingFlags.Static));
            var importedRealCall = HelperDefinition.Module.Import(TargetMethod);

            if (getExecutingAssembly.IsNull()) throw new Exception("Failed to find Assembly.GetExecutingAssembly");

            if (refRequals.IsNull())
                throw new Exception("Failed to find System.Object.ReferenceEquals in module type system");

            if (importedRealCall.IsNull())
                throw new Exception("Failed to import target method " + TargetMethod.Name);


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
            LoadReturnValueOntoStack(processor);
            processor.Emit(OpCodes.Ret);
            processor.Append(jumpLocation);
            processor.Emit(OpCodes.Callvirt, assemblyCodeBaseCall);
            processor.Emit(OpCodes.Ret);
        }


        protected abstract void LoadReturnValueOntoStack(ILProcessor processor);
    }
}
