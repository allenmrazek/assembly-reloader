extern alias Cecil96;
using System;
using System.IO;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using AssemblyDefinition = Cecil96::Mono.Cecil.AssemblyDefinition;
using BaseAssemblyResolver = Cecil96::Mono.Cecil.BaseAssemblyResolver;
using ReaderParameters = Cecil96::Mono.Cecil.ReaderParameters;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    [Implements, Implements(typeof(IAssemblyDefinitionReader))]
// ReSharper disable once UnusedMember.Global
    public class AssemblyDefinitionReader : IAssemblyDefinitionReader
    {
        private readonly IFile _definitionFile;
        private readonly IGetDebugSymbolsExistForDefinition _getDebugSymbolsExistQuery;
        private readonly BaseAssemblyResolver _resolver;
        private readonly ILog _log;


        public AssemblyDefinitionReader(
            IFile definitionFile,
            IGetDebugSymbolsExistForDefinition getDebugSymbolsExistQuery,
            BaseAssemblyResolver resolver,
            ILog log)
        {
            if (definitionFile == null) throw new ArgumentNullException("definitionFile");
            if (getDebugSymbolsExistQuery == null) throw new ArgumentNullException("getDebugSymbolsExistQuery");
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (log == null) throw new ArgumentNullException("log");

            _definitionFile = definitionFile;
            _getDebugSymbolsExistQuery = getDebugSymbolsExistQuery;
            _resolver = resolver;
            _log = log;
        }


        public Maybe<AssemblyDefinition> Read()
        {
            _log.Debug("Reading assembly definition from {0}", _definitionFile.Url);

            if (!File.Exists(_definitionFile.FullPath))
            {
                _log.Error("No definition found at \"{0}\"", _definitionFile.FullPath);
                return Maybe<AssemblyDefinition>.None;
            }

            var readerParameters = ConfigureReaderParameters(_definitionFile);
            _log.Debug("Debug symbols found: " + (readerParameters.ReadSymbols ? "YES" : "NO"));

            var definition = AssemblyDefinition.ReadAssembly(_definitionFile.FullPath, readerParameters);

            if (definition != null)
                _log.Debug("Successfully read definition");
            else _log.Error("Failed to read definition");

            return definition.IsNull() ? Maybe<AssemblyDefinition>.None : Maybe<AssemblyDefinition>.With(definition);
        }


        private ReaderParameters ConfigureReaderParameters(IFile location)
        {
            return new ReaderParameters(Cecil96::Mono.Cecil.ReadingMode.Immediate)
            {
                ReadSymbols = _getDebugSymbolsExistQuery.Get(location),
                AssemblyResolver = _resolver
            };
        }
    }
}
