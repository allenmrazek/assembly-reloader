using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Queries.FileSystemQueries;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Definition
{
    public class AssemblyDefinitionFromDiskReader : IAssemblyDefinitionProvider
    {
        private readonly IDebugSymbolFileExistsQuery _debugSymbolsExistQuery;
        private readonly BaseAssemblyResolver _resolver;


        public AssemblyDefinitionFromDiskReader(
            [NotNull] IDebugSymbolFileExistsQuery debugSymbolsExistQuery,
            [NotNull] BaseAssemblyResolver resolver)
        {
            if (debugSymbolsExistQuery == null) throw new ArgumentNullException("debugSymbolsExistQuery");
            if (resolver == null) throw new ArgumentNullException("resolver");

            _debugSymbolsExistQuery = debugSymbolsExistQuery;
            _resolver = resolver;
        }


        public Maybe<Mono.Cecil.AssemblyDefinition> Get([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            var definition = Mono.Cecil.AssemblyDefinition.ReadAssembly(location.FullPath, ConfigureReaderParameters());

            return definition.IsNull() ? Maybe<Mono.Cecil.AssemblyDefinition>.None : Maybe<Mono.Cecil.AssemblyDefinition>.With(definition);
        }


        private ReaderParameters ConfigureReaderParameters()
        {
            return new ReaderParameters(ReadingMode.Immediate)
            {
                ReadSymbols = _debugSymbolsExistQuery.Get(),
                AssemblyResolver = _resolver
            };
        }
    }
}
