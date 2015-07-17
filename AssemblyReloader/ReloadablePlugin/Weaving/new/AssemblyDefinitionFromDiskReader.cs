//using System;
//using AssemblyReloader.Properties;
//using Mono.Cecil;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.FileSystem;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.ReloadablePlugin.Weaving.@new
//{
//    public class AssemblyDefinitionFromDiskReader : IAssemblyDefinitionProvider
//    {
//        private readonly IGetDebugSymbolsExistForDefinition _getDebugSymbolsExistQuery;
//        private readonly BaseAssemblyResolver _resolver;
//        private readonly ILog _log;


//        public AssemblyDefinitionFromDiskReader(
//            [NotNull] IGetDebugSymbolsExistForDefinition getDebugSymbolsExistQuery,
//            [NotNull] BaseAssemblyResolver resolver,
//            [NotNull] ILog log)
//        {
//            if (getDebugSymbolsExistQuery == null) throw new ArgumentNullException("getDebugSymbolsExistQuery");
//            if (resolver == null) throw new ArgumentNullException("resolver");
//            if (log == null) throw new ArgumentNullException("log");

//            _getDebugSymbolsExistQuery = getDebugSymbolsExistQuery;
//            _resolver = resolver;
//            _log = log;
//        }


//        public Maybe<AssemblyDefinition> Get([NotNull] IFile location)
//        {
//            if (location == null) throw new ArgumentNullException("location");

//            var readerParameters = ConfigureReaderParameters(location);
//            _log.Debug("Debug symbols found: " + (readerParameters.ReadSymbols ? "YES" : "NO"));

//            var definition = AssemblyDefinition.ReadAssembly(location.FullPath, readerParameters);

//            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
//        }


//        private ReaderParameters ConfigureReaderParameters(IFile location)
//        {
//            return new ReaderParameters(ReadingMode.Immediate)
//            {
//                ReadSymbols = _getDebugSymbolsExistQuery.Get(location),
//                AssemblyResolver = _resolver
//            };
//        }
//    }
//}
