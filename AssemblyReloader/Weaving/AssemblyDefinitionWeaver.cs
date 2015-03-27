using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Queries.CecilQueries;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.Weaving
{
    public class AssemblyDefinitionWeaver : IAssemblyDefinitionWeaver
    {
        private readonly ILog _log;
        private readonly ITypeDefinitionQuery _allTypeDefinitionsQuery;
        private readonly IMethodDefinitionQuery _allMethodDefinitionsQuery;
        private readonly IWeaveOperation[] _operations;



        public AssemblyDefinitionWeaver(
            ILog log, 
            ITypeDefinitionQuery allTypeDefinitionsQuery,
            IMethodDefinitionQuery allMethodDefinitionsQuery,
            params IWeaveOperation[] operations)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (allTypeDefinitionsQuery == null) throw new ArgumentNullException("allTypeDefinitionsQuery");
            if (allMethodDefinitionsQuery == null) throw new ArgumentNullException("allMethodDefinitionsQuery");
            if (operations == null) throw new ArgumentNullException("operations");

            _log = log;
            _allTypeDefinitionsQuery = allTypeDefinitionsQuery;
            _allMethodDefinitionsQuery = allMethodDefinitionsQuery;
            _operations = operations;
        }


        public bool Weave(AssemblyDefinition assemblyDefinition)
        {
            try
            {
                var types = _allTypeDefinitionsQuery.Get(assemblyDefinition).ToList();

                var allTypeDefinitions = _allTypeDefinitionsQuery.Get(assemblyDefinition).ToList();
                var allMethodDefinitions = types.SelectMany(td => _allMethodDefinitionsQuery.Get(td)).ToList();

                return _operations.All(op => RunOperation(op, assemblyDefinition, allTypeDefinitions, allMethodDefinitions));
            }
            catch (Exception e)
            {
                _log.Error("Weaving failed: " + e);
                throw;
            }
        }


        private bool RunOperation(
            IWeaveOperation op, 
            AssemblyDefinition definition, 
            List<TypeDefinition> typeDefinitions, 
            List<MethodDefinition> methodDefinitions)
        {
            if (op == null) throw new ArgumentNullException("op");
            if (definition == null) throw new ArgumentNullException("definition");

            try
            {
                op.Run(definition);

                typeDefinitions.ForEach(td => op.OnEachType(td));
                methodDefinitions.ForEach(md => op.OnEachMethod(md));

                return true;
            }
            catch (Exception e)
            {
                _log.Error("Weave operation " + op.GetType().FullName + " failed due to: " + e);
                return false;
            }
        }
    }
}
