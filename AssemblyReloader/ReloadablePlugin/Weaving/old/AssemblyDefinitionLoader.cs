//extern alias Cecil96;
//using System;
//using System.IO;
//using System.Reflection;
//using AssemblyReloader.Properties;
//using ReeperCommon.Containers;
//using ReeperCommon.Extensions;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.ReloadablePlugin.Weaving.old
//{
//// ReSharper disable once ClassNeverInstantiated.Global
//    public class AssemblyDefinitionLoader : IAssemblyDefinitionLoader
//    {
//        private const int InitialMemoryStreamSize = 1024 * 1024;

//        private readonly IGetTemporaryFile _tempFile;
//        private readonly ILog _log;


//        public AssemblyDefinitionLoader(
//            [NotNull] IGetTemporaryFile tempFile, 
//            [NotNull] ILog log)
//        {
//            if (tempFile == null) throw new ArgumentNullException("tempFile");
//            if (log == null) throw new ArgumentNullException("log");

//            _tempFile = tempFile;
//            _log = log;
//        }


//        public Maybe<Assembly> Get([NotNull] Cecil96::Mono.Cecil.AssemblyDefinition definition)
//        {
//            if (definition == null) throw new ArgumentNullException("definition");

//            using (var byteStream = new MemoryStream(InitialMemoryStreamSize))
//            using (var symbolStream = new MemoryStream(InitialMemoryStreamSize))
//            {
//                if (definition.MainModule.HasSymbols)
//                    WriteDefinitionAndSymbolsToStream(definition, byteStream, symbolStream);
//                else WriteDefinitionToStream(definition, byteStream);


//                if (byteStream.Length != 0) return LoadAssembly(byteStream, symbolStream);

//                _log.Error("byteStream does not contain any data for " + definition.FullName);
//                return Maybe<Assembly>.None;
//            }
//        }


//        /// <summary>
//        /// For who knows why the hell why, Cecil can read mdbs from a stream but not write them (see
//        /// https://github.com/jbevain/cecil/blob/master/symbols/mdb/Mono.Cecil.Mdb/MdbWriter.cs )
//        /// 
//        /// As a workaround, dump the contents to disk and read it back into memory
//        /// </summary>
//        /// <param name="definition"></param>
//        /// <param name="assemblyStream"></param>
//        /// <param name="symbolStream"></param>
//        private void WriteDefinitionAndSymbolsToStream(
//            [NotNull] Cecil96::Mono.Cecil.AssemblyDefinition definition,
//            [NotNull] Stream assemblyStream, 
//            [NotNull] Stream symbolStream)
//        {
//            if (definition == null) throw new ArgumentNullException("definition");
//            if (assemblyStream == null) throw new ArgumentNullException("assemblyStream");
//            if (symbolStream == null) throw new ArgumentNullException("symbolStream");

//            if (!definition.MainModule.HasSymbols)
//                throw new ArgumentException("definition does not contain debug symbols");

//            using (var tempDll = _tempFile.Get())
//            using (var tempSymbolMdb = _tempFile.Get(tempDll.FullPath + ".mdb"))
//            {
//                try
//                {
//                    _log.Debug("Writing temp dll to " + tempDll.FullPath);
//                    _log.Debug("Writing any symbols to " + tempSymbolMdb.FullPath);


//                    definition.Write(tempDll.FullPath, new Cecil96::Mono.Cecil.WriterParameters
//                    {
//                        WriteSymbols = false,
//                    });

//                    _log.Warning("Symbol loading has been intentionally disabled (has a bug I need to find)");

//                    using (var tempFile = new FileStream(tempDll.FullPath, FileMode.Open))
//                    {
//                        var tempSymbolFile = tempSymbolMdb.Stream;

//                        if (tempFile.Length == 0)
//                            throw new Exception("Failed to write assembly definition to disk at " + tempDll.FullPath);

//                        _log.Debug("Copying temp dll into byte stream");
//                        tempFile.CopyTo(assemblyStream);

//                        if (tempSymbolFile.Length == 0)
//                        {
//                            _log.Warning("Failed to write " + definition.FullName + " symbols to " + tempDll.FullPath);
//                        }
//                        else
//                        {
//                            _log.Debug("Copying symbols into byte stream");
//                            tempSymbolFile.CopyTo(symbolStream);
//                        }
//                    }
//                }
//                catch (Cecil96::Mono.CompilerServices.SymbolWriter.MonoSymbolFileException symbolException)
//                {
//                    _log.Warning("Symbol file exception: " + symbolException);
//                    symbolStream.Seek(0, SeekOrigin.Begin);
//                    symbolStream.SetLength(0);

//                    if (assemblyStream.Length == 0)
//                        throw;
//                }
//                catch (FileNotFoundException)
//                {
//                    _log.Error("Could not load assembly from temp file (file does not exist): " + tempDll.FullPath);
//                    throw;
//                } 
//            }
//        }


//        private void WriteDefinitionToStream([NotNull] Cecil96::Mono.Cecil.AssemblyDefinition definition, [NotNull] Stream assemblyStream)
//        {
//            if (definition == null) throw new ArgumentNullException("definition");
//            if (assemblyStream == null) throw new ArgumentNullException("assemblyStream");
//            if (definition.MainModule.HasSymbols)
//                _log.Warning("Ignoring " + definition.FullName + " symbols (is this intended?)");

//            definition.Write(assemblyStream);

//            if (assemblyStream.Length == 0)
//                throw new Exception("Failed to write " + definition.FullName + " to memory stream");
//        }


//        private Maybe<Assembly> LoadAssembly(
//            [NotNull] MemoryStream byteStream,
//            [NotNull] MemoryStream symbolStream)
//        {
//            if (byteStream == null) throw new ArgumentNullException("byteStream");
//            if (symbolStream == null) throw new ArgumentNullException("symbolStream");
//            if (byteStream.Length == 0) throw new InvalidOperationException("byteStream does not contain any data");

//            var assembly = symbolStream.Length > 0 ? 
//                Assembly.Load(byteStream.GetBuffer(), symbolStream.GetBuffer())
//                : Assembly.Load(byteStream.GetBuffer());

//            if (assembly == null)
//            {
//                _log.Warning("Failed to load assembly from byte stream");
//            }
//            else _log.Verbose("Loaded assembly from stream (debug symbols = " + (symbolStream.Length > 0 ? "YES" : "NO") + ")");

//            return assembly == null ? Maybe<Assembly>.None : Maybe<Assembly>.With(assembly);
//        }
//    }
//}