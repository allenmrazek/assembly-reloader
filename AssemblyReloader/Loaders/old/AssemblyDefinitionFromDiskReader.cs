//using System;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Queries.FileSystemQueries;
//using Mono.Cecil;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.FileSystem;

//namespace AssemblyReloader.Loaders
//{
//    public class AssemblyDefinitionFromDiskReader : IAssemblyDefinitionReader
//    {
//        private readonly IDebugSymbolFileExistsQuery _debugSymbolsExistQuery;
//        private readonly BaseAssemblyResolver _resolver;


//        public AssemblyDefinitionFromDiskReader(
//            [NotNull] IFile location, 
//            [NotNull] IDebugSymbolFileExistsQuery debugSymbolsExistQuery, 
//            [NotNull] BaseAssemblyResolver resolver)
//        {
//            if (location == null) throw new ArgumentNullException("location");
//            if (debugSymbolsExistQuery == null) throw new ArgumentNullException("debugSymbolsExistQuery");
//            if (resolver == null) throw new ArgumentNullException("resolver");

//            _debugSymbolsExistQuery = debugSymbolsExistQuery;
//            _resolver = resolver;
//            Location = location;
//        }


//        public Maybe<AssemblyDefinition> Get()
//        {
//            var definition = AssemblyDefinition.ReadAssembly(Location.FullPath, ConfigureReaderParameters());

//            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
//        }


//        private ReaderParameters ConfigureReaderParameters()
//        {
//            return new ReaderParameters(ReadingMode.Immediate)
//            {
//                ReadSymbols = _debugSymbolsExistQuery.Get(),
//                AssemblyResolver = _resolver
//            };
//        }


//        public IFile Location { get; private set; }
//    }
//}
