//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AssemblyReloader.Properties;
//using AssemblyReloader.ReloadablePlugin.Weaving;
//using Mono.Cecil;
//using ReeperCommon.Containers;
//using ReeperCommon.FileSystem;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.ReloadablePlugin.Definition
//{
//    public class AssemblyDefinitionWeaver : IAssemblyDefinitionProvider
//    {
//        private readonly IAssemblyDefinitionProvider _decorated;
//        private readonly IGetTypeDefinitions _allGetTypeDefinitionsQuery;
//        private readonly IGetMethodDefinitions _allMethodDefinitionsQuery;
//        private readonly ILog _log;
//        private readonly IEnumerable<IWeaveOperation> _operations;


//        public AssemblyDefinitionWeaver(
//            [NotNull] IAssemblyDefinitionProvider decorated,
//            [NotNull] IGetTypeDefinitions allGetTypeDefinitionsQuery,
//            [NotNull] IGetMethodDefinitions allMethodDefinitionsQuery,
//            [NotNull] ILog log,
//            [NotNull] params IWeaveOperation[] operations)
//        {
//            if (decorated == null) throw new ArgumentNullException("decorated");
//            if (allGetTypeDefinitionsQuery == null) throw new ArgumentNullException("allGetTypeDefinitionsQuery");
//            if (allMethodDefinitionsQuery == null) throw new ArgumentNullException("allMethodDefinitionsQuery");
//            if (log == null) throw new ArgumentNullException("log");
//            if (operations == null) throw new ArgumentNullException("operations");

//            _decorated = decorated;
//            _allGetTypeDefinitionsQuery = allGetTypeDefinitionsQuery;
//            _allMethodDefinitionsQuery = allMethodDefinitionsQuery;
//            _log = log;
//            _operations = operations;
//        }


//        public Maybe<Context> Get([NotNull] IFile location)
//        {
//            if (location == null) throw new ArgumentNullException("location");
//            var result = _decorated.Get(location);

//            if (result.Any())
//                return Weave(result.Single()) ? result : Maybe<Context>.None;

//            return result;
//        }


//        private bool Weave([NotNull] Context assemblyDefinition)
//        {
//            if (assemblyDefinition == null) throw new ArgumentNullException("assemblyDefinition");

//            _log.Verbose("Weaving " + assemblyDefinition.FullName);

//            try
//            {
//                var allTypeDefinitions = _allGetTypeDefinitionsQuery.Get(assemblyDefinition).ToList();
//                _log.Verbose("Found " + allTypeDefinitions.Count + " type definitions in assembly definition");

//                var allMethodDefinitions = allTypeDefinitions.SelectMany(td => _allMethodDefinitionsQuery.Get(td)).ToList();
//                _log.Verbose("Found " + allMethodDefinitions.Count + " method definitions in assembly definition");

//                var result = _operations.All(op => RunOperation(op, assemblyDefinition, allTypeDefinitions, allMethodDefinitions));

//                if (!result)
//                    _log.Warning("One or more weave operations failed!");

//                return result;
//            }
//            catch (Exception e)
//            {
//                _log.Error("Weaving failed: " + e);
//                throw;
//            }
//        }


//        private bool RunOperation(
//            [NotNull] IWeaveOperation op, 
//            [NotNull] Context definition, 
//            [NotNull] List<TypeDefinition> typeDefinitions,
//            [NotNull] List<MethodDefinition> methodDefinitions)
//        {
//            if (op == null) throw new ArgumentNullException("op");
//            if (definition == null) throw new ArgumentNullException("definition");
//            if (typeDefinitions == null) throw new ArgumentNullException("typeDefinitions");
//            if (methodDefinitions == null) throw new ArgumentNullException("methodDefinitions");

//            try
//            {
//                op.Run(definition);

//                typeDefinitions.ForEach(op.OnEachType);
//                methodDefinitions.ForEach(op.OnEachMethod);

//                return true;
//            }
//            catch (Exception ex)
//            {
//                _log.Error("Weave operation " + op.GetType().FullName + " failed due to: " + ex);
//                return false;
//            }
//        }
//    }
//}
