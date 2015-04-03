using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Queries.FileSystemQueries;
using Mono.Cecil;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionMemoryLoader : IAssemblyDefinitionMemoryLoader
    {
        private readonly ILog _log;
        private readonly IDebugSymbolFileExistsQuery _debugSymbolExistsQuery;
        private const int InitialMemoryStreamSize = 1024 * 1024;


        public AssemblyDefinitionMemoryLoader(
            [NotNull] ILog log,
            [NotNull] IDebugSymbolFileExistsQuery debugSymbolExistsQuery)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (debugSymbolExistsQuery == null) throw new ArgumentNullException("debugSymbolExistsQuery");

            _log = log;
            _debugSymbolExistsQuery = debugSymbolExistsQuery;
        }


        public Maybe<Assembly> LoadDefinition([NotNull] AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            using (var byteStream = new MemoryStream(InitialMemoryStreamSize))
            using (var symbolStream = new MemoryStream(InitialMemoryStreamSize))
            {
                definition.Write(byteStream, ConfigureWriterParameters(symbolStream));

                if (byteStream.Length == 0) throw new ArgumentException("byteStream does not contain any data");

                var assembly = symbolStream.Length > 0
                    ? LoadAssemblyWithSymbols(byteStream, symbolStream)
                    : LoadAssemblyWithoutSymbols(byteStream);

                return assembly != null ? Maybe<Assembly>.With(assembly) : Maybe<Assembly>.None;
            }
        }


        private Assembly LoadAssemblyWithSymbols(
            [NotNull] MemoryStream byteStream,
            [NotNull] MemoryStream symbolStream)
        {
            if (byteStream == null) throw new ArgumentNullException("byteStream");
            if (symbolStream == null) throw new ArgumentNullException("symbolStream");

            var assembly = Assembly.Load(byteStream.GetBuffer(), symbolStream.GetBuffer());

            if (assembly == null)
                _log.Warning("Failed to load assembly with symbols from byte stream");
            else _log.Normal("Loaded assembly from stream with debug symbols");

            return assembly;
        }


        private Assembly LoadAssemblyWithoutSymbols([NotNull] MemoryStream byteStream)
        {
            if (byteStream == null) throw new ArgumentNullException("byteStream");


            var assembly = Assembly.Load(byteStream.GetBuffer());

            if (assembly == null)
                _log.Warning("Failed to load assembly without symbols from byte stream");

            return assembly;
        }


        private WriterParameters ConfigureWriterParameters([CanBeNull] MemoryStream symbolStream)
        {
            return new WriterParameters { WriteSymbols = _debugSymbolExistsQuery.Get(), SymbolStream = symbolStream };
        }
    }
}
