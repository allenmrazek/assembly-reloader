using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
    [Implements(typeof(IAssemblyDefinitionReader))]
// ReSharper disable once UnusedMember.Global
    public class AssemblyDefinitionReader : IAssemblyDefinitionReader
    {
        private readonly IGetDebugSymbolsExistForDefinition _getDebugSymbolsExistQuery;
        private readonly BaseAssemblyResolver _resolver;
        private readonly ILog _log;


        public AssemblyDefinitionReader(
            [NotNull] IGetDebugSymbolsExistForDefinition getDebugSymbolsExistQuery,
            [NotNull] BaseAssemblyResolver resolver,
            [NotNull] ILog log)
        {
            if (getDebugSymbolsExistQuery == null) throw new ArgumentNullException("getDebugSymbolsExistQuery");
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (log == null) throw new ArgumentNullException("log");

            _getDebugSymbolsExistQuery = getDebugSymbolsExistQuery;
            _resolver = resolver;
            _log = log;
        }


        public Maybe<AssemblyDefinition> Read(IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            _log.Debug("Reading assembly definition from {0}", location.Url);

            var readerParameters = ConfigureReaderParameters(location);
            _log.Debug("Debug symbols found: " + (readerParameters.ReadSymbols ? "YES" : "NO"));

            var definition = AssemblyDefinition.ReadAssembly(location.FullPath, readerParameters);

            if (definition != null)
                _log.Debug("Successfully read definition");
            else _log.Error("Failed to read definition");

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
