using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Weaving.old.Definition;
using AssemblyReloader.ReloadablePlugin.Weaving.Operations;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving.old
{
    public class AssemblyDefinitionWeaver : IAssemblyDefinitionWeaver
    {
        private readonly ILog _log;
        private readonly IGetTypeDefinitions _allGetTypeDefinitionsQuery;
        private readonly IGetMethodDefinitions _allGetMethodDefinitionses;
        private readonly IWeaveOperation[] _operations;



        public AssemblyDefinitionWeaver(
            ILog log, 
            IGetTypeDefinitions allGetTypeDefinitionsQuery,
            IGetMethodDefinitions allGetMethodDefinitionses,
            params IWeaveOperation[] operations)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (allGetTypeDefinitionsQuery == null) throw new ArgumentNullException("allGetTypeDefinitionsQuery");
            if (allGetMethodDefinitionses == null) throw new ArgumentNullException("allGetMethodDefinitionses");
            if (operations == null) throw new ArgumentNullException("operations");

            _log = log;
            _allGetTypeDefinitionsQuery = allGetTypeDefinitionsQuery;
            _allGetMethodDefinitionses = allGetMethodDefinitionses;
            _operations = operations;
        }


        public bool Weave(AssemblyDefinition assemblyDefinition)
        {
            _log.Verbose("Weaving " + assemblyDefinition.FullName);

            try
            {
                var allTypeDefinitions = _allGetTypeDefinitionsQuery.Get(assemblyDefinition).ToList();
                _log.Verbose("Found " + allTypeDefinitions.Count + " type definitions in assembly definition");

                var allMethodDefinitions = allTypeDefinitions.SelectMany(td => _allGetMethodDefinitionses.Get(td)).ToList();
                _log.Verbose("Found " + allMethodDefinitions.Count + " method definitions in assembly definition");

                var result = _operations.All(op => RunOperation(op, assemblyDefinition, allTypeDefinitions, allMethodDefinitions));

                if (!result)
                    _log.Warning("One or more weave operations failed!");

                return result;
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
