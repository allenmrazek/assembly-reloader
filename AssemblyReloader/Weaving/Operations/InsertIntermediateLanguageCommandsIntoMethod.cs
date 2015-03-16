using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.Weaving.Operations
{
    public class InsertIntermediateLanguageCommandsIntoMethod : WeaveOperation
    {
        private readonly ILog _log;
        private readonly ITypeDefinitionQuery _typeDefinitionQuery;
        private readonly IMethodDefinitionQuery _targetMethodQuery;

        public InsertIntermediateLanguageCommandsIntoMethod(
            ILog log,
            ITypeDefinitionQuery typeDefinitionQuery,
            IMethodDefinitionQuery targetMethodQuery)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (typeDefinitionQuery == null) throw new ArgumentNullException("typeDefinitionQuery");
            if (targetMethodQuery == null) throw new ArgumentNullException("targetMethodQuery");

            _log = log;
            _typeDefinitionQuery = typeDefinitionQuery;
            _targetMethodQuery = targetMethodQuery;
        }


        public override void Run(AssemblyDefinition definition)
        {
            _typeDefinitionQuery.Get(definition)
                .SelectMany(td => _targetMethodQuery.Get(td))
                .ToList()
                .ForEach(InsertILInto);

            _typeDefinitionQuery.Get(definition).ToList().ForEach(td => _log.Normal("TypeDefinition: " + td.FullName));
        }


// ReSharper disable once InconsistentNaming
        private void InsertILInto(MethodDefinition methodDefinition)
        {
            if (methodDefinition == null) throw new ArgumentNullException("methodDefinition");

            _log.Normal("Inserting IL into " + methodDefinition.FullName);

        }
    }
}
