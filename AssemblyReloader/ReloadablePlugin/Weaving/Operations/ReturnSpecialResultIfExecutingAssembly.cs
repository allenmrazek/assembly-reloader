using System;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;
using System.Linq;
using ReeperCommon.Extensions;
using MethodAttributes = Mono.Cecil.MethodAttributes;
using ParameterAttributes = Mono.Cecil.ParameterAttributes;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public abstract class ReturnSpecialResultIfExecutingAssembly : RewriteCall
    {
        private MethodDefinition replacementMethodDefinition;


        public ReturnSpecialResultIfExecutingAssembly(
            MethodInfo targetMethod, 
            IGetInstructionsInMethod callsToInterceptedMethodQuery, 
            IGetTypeDefinitions typeDefinitionQuery) : 
            base(targetMethod, callsToInterceptedMethodQuery, typeDefinitionQuery)
        {
        }


        public override void Execute()
        {
            replacementMethodDefinition = CreateProxyMethod();
            base.Execute();
        }


        protected override void ReplaceOriginalCallWithProxy(MethodDefinition inMethod, Instruction callInstruction)
        {
            Log.Verbose("Replacing call at " + callInstruction.Offset + " in " + inMethod.FullName + " with " +
                       replacementMethodDefinition.FullName);

            var processor = inMethod.Body.GetILProcessor();

            processor.Replace(callInstruction, Instruction.Create(OpCodes.Call, inMethod.Module.Import(replacementMethodDefinition)));
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
                HelperDefinition.Module.Import(typeof(System.Object).GetMethod("ReferenceEquals",
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
