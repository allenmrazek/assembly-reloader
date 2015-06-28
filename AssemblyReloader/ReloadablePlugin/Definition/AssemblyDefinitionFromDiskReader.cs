using System;
using AssemblyReloader.Properties;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
    public class AssemblyDefinitionFromDiskReader : IAssemblyDefinitionProvider
    {
        private readonly IGetDebugSymbolsExistForDefinition _getDebugSymbolsExistQuery;
        private readonly BaseAssemblyResolver _resolver;


        public AssemblyDefinitionFromDiskReader(
            [NotNull] IGetDebugSymbolsExistForDefinition getDebugSymbolsExistQuery,
            [NotNull] BaseAssemblyResolver resolver)
        {
            if (getDebugSymbolsExistQuery == null) throw new ArgumentNullException("getDebugSymbolsExistQuery");
            if (resolver == null) throw new ArgumentNullException("resolver");

            _getDebugSymbolsExistQuery = getDebugSymbolsExistQuery;
            _resolver = resolver;
        }


        public Maybe<AssemblyDefinition> Get([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            var definition = AssemblyDefinition.ReadAssembly(location.FullPath, ConfigureReaderParameters(location));

            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
        }


        private ReaderParameters ConfigureReaderParameters(IFile location)
        {
            return new ReaderParameters(ReadingMode.Immediate)
            {
                ReadSymbols = _getDebugSymbolsExistQuery.Get(location),
                AssemblyResolver = _resolver
            };
        }
    }
}
