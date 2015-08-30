extern alias Cecil96;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Logging;
using strange.extensions.command.impl;
using Instruction = Cecil96::Mono.Cecil.Cil.Instruction;
using MethodDefinition = Cecil96::Mono.Cecil.MethodDefinition;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using TypeDefinition = Cecil96::Mono.Cecil.TypeDefinition;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations
{
    public abstract class RewriteCall : Command
    {
        protected readonly MethodInfo TargetMethod;
        private readonly IGetInstructionsInMethod _callsToInterceptedMethodQuery;
        private readonly IGetTypeDefinitions _typeDefinitionQuery;

        [Inject] public AssemblyDefinition Context { get; set; }
        [Inject] public TypeDefinition HelperDefinition { get; set; }
        [Inject] public ILog Log { get; set; }


        public RewriteCall(
            MethodInfo targetMethod,
            IGetInstructionsInMethod callsToInterceptedMethodQuery,
            IGetTypeDefinitions typeDefinitionQuery)
        {
            if (targetMethod == null) throw new ArgumentNullException("targetMethod");
            if (callsToInterceptedMethodQuery == null) throw new ArgumentNullException("callsToInterceptedMethodQuery");
            if (typeDefinitionQuery == null) throw new ArgumentNullException("typeDefinitionQuery");

            TargetMethod = targetMethod;
            _callsToInterceptedMethodQuery = callsToInterceptedMethodQuery;
            _typeDefinitionQuery = typeDefinitionQuery;
        }


        public override void Execute()
        {
            foreach (var method in GetMethodsContainingTargetInstructions())
            {
                var calls = _callsToInterceptedMethodQuery.Get(method).ToArray();

                // don't use foreach: collection is going to be modified
                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < calls.Length; ++i)
                    ReplaceOriginalCallWithProxy(method, calls[i]);
            }
        }


        private IEnumerable<MethodDefinition> GetMethodsContainingTargetInstructions()
        {
            return _typeDefinitionQuery.Get(Context)
                .SelectMany(
                    td => td.Methods
                        .Where(md => _callsToInterceptedMethodQuery.Get(md).Any()));
        }


        protected abstract void ReplaceOriginalCallWithProxy(MethodDefinition inMethod, Instruction callInstruction);
    }
}
