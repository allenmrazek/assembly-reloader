using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Generators;
using AssemblyReloader.Queries.FileSystemQueries;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Mdb;
using Mono.CompilerServices.SymbolWriter;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public class AssemblyDefinitionMemoryLoader : IAssemblyDefinitionMemoryLoader
    {
        private readonly ITemporaryFileGenerator _tempFileGenerator;
        private readonly ILog _log;
        private const int InitialMemoryStreamSize = 1024 * 1024;


        public AssemblyDefinitionMemoryLoader(
            [NotNull] ITemporaryFileGenerator tempFileGenerator,
            [NotNull] ILog log)
        {
            if (tempFileGenerator == null) throw new ArgumentNullException("tempFileGenerator");
            if (log == null) throw new ArgumentNullException("log");

            _tempFileGenerator = tempFileGenerator;
            _log = log;
        }


        public Maybe<Assembly> LoadDefinition([NotNull] AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            using (var byteStream = new MemoryStream(InitialMemoryStreamSize))
            using (var symbolFile = _tempFileGenerator.Get())
            {
                try
                {
                    // note: we can't use stream AT ALL; it's completely not implemented. Have to use two temp files
                    definition.Write(byteStream, ConfigureWriterParameters(definition, symbolFile.Stream));

                    // tested and works
                    //definition.Write("D:/TestDefinition.dll",
                    //    new WriterParameters {WriteSymbols = true, SymbolWriterProvider = new MdbWriterProvider()});
                }
                catch (MonoSymbolFileException e)
                {
                    _log.Error("Caught MonoSymbolFileException while writing AssemblyDefinition to stream: " + e);
                    throw;
                }
                
                


                if (byteStream.Length == 0)
                {
                    _log.Error("byteStream does not contain any data for " + definition.FullName);
                    return Maybe<Assembly>.None;
                }

                // todo: use temp file to load symbols
                var assembly = /*symbolStream.Length > 0
                    ? LoadAssemblyWithSymbols(byteStream, symbolStream)
                    :*/ LoadAssemblyWithoutSymbols(byteStream);

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
                _log.Warning("Failed to load assembly without debug symbols from byte stream");
            else _log.Normal("Loaded assembly from stream without debug symbols");
           
            return assembly;
        }


        private WriterParameters ConfigureWriterParameters(
            [NotNull] AssemblyDefinition definition,
            [CanBeNull] Stream symbolStream)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            return new WriterParameters
            {
                WriteSymbols = definition.MainModule.HasSymbols, 
                SymbolStream = symbolStream
            };
        }
    }
}
