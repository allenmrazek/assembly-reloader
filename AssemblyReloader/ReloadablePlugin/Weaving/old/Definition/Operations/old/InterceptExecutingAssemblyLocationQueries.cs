using System;
using System.Linq;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AssemblyReloader.ReloadablePlugin.Definition.Operations.old
{
    public class InterceptExecutingAssemblyLocationQueries : WeaveOperation
    {
        private readonly IInstructionSetQuery _getCodeBaseCallQuery;
        private readonly IGetMethodDefinitions _replacementCall;

        public InterceptExecutingAssemblyLocationQueries(
            IInstructionSetQuery getCodeBaseCallQuery,
            IGetMethodDefinitions replacementCall)
        {
            if (getCodeBaseCallQuery == null) throw new ArgumentNullException("getCodeBaseCallQuery");
            if (replacementCall == null) throw new ArgumentNullException("replacementCall");

            _getCodeBaseCallQuery = getCodeBaseCallQuery;
            _replacementCall = replacementCall;

        }


        public override void Run(AssemblyDefinition definition)
        {
            // do nothing
        }


        public override void OnEachMethod(MethodDefinition methodDefinition)
        {
            var results = _getCodeBaseCallQuery.Get(methodDefinition).ToList();
            if (results.Count == 0) return;

            var processor = methodDefinition.Body.GetILProcessor();
            var replacement = _replacementCall.Get(methodDefinition.DeclaringType).ToList();

            if (!replacement.Any())
                throw new Exception("Failed to locate replacement call");

            if (replacement.Count > 1)
                throw new Exception("Found too many replacement methods");

            results.ForEach(call => processor.Replace(call, processor.Create(OpCodes.Call, replacement.Single())));
        }
    }
}
