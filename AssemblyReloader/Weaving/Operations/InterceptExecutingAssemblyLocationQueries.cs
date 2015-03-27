using System;
using System.Linq;
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Queries.CecilQueries.IntermediateLanguage;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace AssemblyReloader.Weaving.Operations
{
    public class InterceptExecutingAssemblyLocationQueries : WeaveOperation
    {
        private readonly IInstructionSetQuery _getCodeBaseCallQuery;
        private readonly IMethodDefinitionQuery _replacementCallQuery;

        public InterceptExecutingAssemblyLocationQueries(
            IInstructionSetQuery getCodeBaseCallQuery,
            IMethodDefinitionQuery replacementCallQuery)
        {
            if (getCodeBaseCallQuery == null) throw new ArgumentNullException("getCodeBaseCallQuery");
            if (replacementCallQuery == null) throw new ArgumentNullException("replacementCallQuery");

            _getCodeBaseCallQuery = getCodeBaseCallQuery;
            _replacementCallQuery = replacementCallQuery;

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
            var replacement = _replacementCallQuery.Get(methodDefinition.DeclaringType).ToList();

            if (!replacement.Any())
                throw new Exception("Failed to locate replacement call");

            if (replacement.Count > 1)
                throw new Exception("Found too many replacement methods");

            results.ForEach(call => processor.Replace(call, processor.Create(OpCodes.Call, replacement.Single())));
        }
    }
}
