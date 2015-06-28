using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Generators;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Definition;
using Mono.Cecil;
using Mono.CompilerServices.SymbolWriter;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.FileSystem
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class AssemblyProvider : IAssemblyProvider
    {
        private const int InitialMemoryStreamSize = 1024 * 1024;

        private readonly IAssemblyDefinitionProvider _definitionProvider;
        private readonly ITemporaryFileFactory _tempFileFactory;
        private readonly ILog _log;


        public AssemblyProvider(
            [NotNull] IAssemblyDefinitionProvider definitionProvider,
            [NotNull] ITemporaryFileFactory tempFileFactory, 
            [NotNull] ILog log)
        {
            if (definitionProvider == null) throw new ArgumentNullException("definitionProvider");
            if (tempFileFactory == null) throw new ArgumentNullException("tempFileFactory");
            if (log == null) throw new ArgumentNullException("log");

            _definitionProvider = definitionProvider;
            _tempFileFactory = tempFileFactory;
            _log = log;
        }


        public Maybe<Assembly> Get([NotNull] IFile definitionLocation)
        {
            if (definitionLocation == null) throw new ArgumentNullException("definitionLocation");

            var definition = _definitionProvider.Get(definitionLocation);

            return definition.Any() ? LoadDefinition(definition.Single()) : Maybe<Assembly>.None;
        }


        private Maybe<Assembly> LoadDefinition([NotNull] AssemblyDefinition definition)
        {
            if (definition == null) throw new ArgumentNullException("definition");

            using (var byteStream = new MemoryStream(InitialMemoryStreamSize))
            using (var symbolStream = new MemoryStream(InitialMemoryStreamSize))
            {
                if (definition.MainModule.HasSymbols)
                    WriteDefinitionAndSymbolsToStream(definition, byteStream, symbolStream);
                else WriteDefinitionToStream(definition, byteStream);


                if (byteStream.Length != 0) return LoadAssembly(byteStream, symbolStream);

                _log.Error("byteStream does not contain any data for " + definition.FullName);
                return Maybe<Assembly>.None;
            }
        }


        /// <summary>
        /// For who knows why the hell why, Cecil can read mdbs from a stream but not write them (see
        /// https://github.com/jbevain/cecil/blob/master/symbols/mdb/Mono.Cecil.Mdb/MdbWriter.cs )
        /// 
        /// As a workaround, dump the contents to disk and read it back into memory
        /// </summary>
        /// <param name="definition"></param>
        /// <param name="assemblyStream"></param>
        /// <param name="symbolStream"></param>
        private void WriteDefinitionAndSymbolsToStream(
            [NotNull] AssemblyDefinition definition,
            [NotNull] Stream assemblyStream, 
            [NotNull] Stream symbolStream)
        {
            if (definition == null) throw new ArgumentNullException("definition");
            if (assemblyStream == null) throw new ArgumentNullException("assemblyStream");
            if (symbolStream == null) throw new ArgumentNullException("symbolStream");

            if (!definition.MainModule.HasSymbols)
                throw new ArgumentException("definition does not contain debug symbols");

            using (var tempDll = _tempFileFactory.Get())
            using (var tempSymbolMdb = _tempFileFactory.Get(tempDll.FullPath + ".mdb"))
            {
                try
                {
                    definition.Write(tempDll.FullPath, new WriterParameters {WriteSymbols = true});

                    using (var tempFile = new FileStream(tempDll.FullPath, FileMode.Open))
                    {
                        var tempSymbolFile = tempSymbolMdb.Stream;

                        if (tempFile.Length == 0)
                            throw new Exception("Failed to write assembly definition to disk at " + tempDll.FullPath);

                        tempFile.CopyTo(assemblyStream);

                        if (tempSymbolFile.Length == 0)
                        {
                            _log.Warning("Failed to write " + definition.FullName + " symbols to " + tempDll.FullPath);
                        }
                        else tempSymbolFile.CopyTo(symbolStream);
                    }
                }
                catch (MonoSymbolFileException symbolException)
                {
                    _log.Warning("Symbol file exception: " + symbolException);
                    symbolStream.Seek(0, SeekOrigin.Begin);
                    symbolStream.SetLength(0);

                    if (assemblyStream.Length == 0)
                        throw;
                }
                catch (FileNotFoundException fnf)
                {
                    _log.Error("Could not load assembly from temp file (file does not exist): " + tempDll.FullPath);
                    throw;
                } 
            }
        }


        private void WriteDefinitionToStream([NotNull] AssemblyDefinition definition, [NotNull] Stream assemblyStream)
        {
            if (definition == null) throw new ArgumentNullException("definition");
            if (assemblyStream == null) throw new ArgumentNullException("assemblyStream");
            if (definition.MainModule.HasSymbols)
                _log.Warning("Ignoring " + definition.FullName + " symbols (is this intended?)");

            definition.Write(assemblyStream);

            if (assemblyStream.Length == 0)
                throw new Exception("Failed to write " + definition.FullName + " to memory stream");
        }


        private Maybe<Assembly> LoadAssembly(
            [NotNull] MemoryStream byteStream,
            [NotNull] MemoryStream symbolStream)
        {
            if (byteStream == null) throw new ArgumentNullException("byteStream");
            if (symbolStream == null) throw new ArgumentNullException("symbolStream");
            if (byteStream.Length == 0) throw new InvalidOperationException("byteStream does not contain any data");

            var assembly = symbolStream.Length > 0 ? 
                Assembly.Load(byteStream.GetBuffer(), symbolStream.GetBuffer())
                : Assembly.Load(byteStream.GetBuffer());

            if (assembly == null)
            {
                _log.Warning("Failed to load assembly from byte stream");
            }
            else _log.Verbose("Loaded assembly from stream (debug symbols = " + (symbolStream.Length > 0 ? "YES" : "NO") + ")");

            return assembly == null ? Maybe<Assembly>.None : Maybe<Assembly>.With(assembly);
        }
    }
}