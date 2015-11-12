extern alias Cecil96;
using System;
using System.IO;
using System.Linq;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using Cecil96::Mono.Cecil;

namespace AssemblyReloader.ReloadablePlugin.Weaving
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class WovenRawAssemblyDataFactory : IRawAssemblyDataFactory
    {
        private readonly IAssemblyResolver _resolver;
        private readonly SignalWeaveDefinition _weaveSignal;
        private readonly IRawAssemblyDataFactory _rawFactory;
        private readonly ITemporaryFileFactory _tempFileFactory;
        private readonly ILog _log;

        public WovenRawAssemblyDataFactory(
            IAssemblyResolver resolver,
            SignalWeaveDefinition weaveSignal, 
            IRawAssemblyDataFactory rawFactory,
            ITemporaryFileFactory tempFileFactory,
            ILog log)
        {
            if (resolver == null) throw new ArgumentNullException("resolver");
            if (weaveSignal == null) throw new ArgumentNullException("weaveSignal");
            if (rawFactory == null) throw new ArgumentNullException("rawFactory");
            if (tempFileFactory == null) throw new ArgumentNullException("tempFileFactory");
            if (log == null) throw new ArgumentNullException("log");
            _resolver = resolver;
            _weaveSignal = weaveSignal;
            _rawFactory = rawFactory;
            _tempFileFactory = tempFileFactory;
            _log = log;
        }


        public RawAssemblyData Create(ReeperAssembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var definition = ReadDefinition(assembly);

            Weave(definition);

            return CreateRawAssemblyDataFromDefinition(assembly, definition);
        }


        private AssemblyDefinition ReadDefinition(ReeperAssembly assembly)
        {
            var raw = _rawFactory.Create(assembly);

            var definition = AssemblyDefinition.ReadAssembly(raw.RawAssembly, new ReaderParameters
            {
                AssemblyResolver = _resolver,
                ReadSymbols = assembly.Symbols.Any(),
                SymbolStream = raw.SymbolStore.SingleOrDefault(),
                ReadingMode = ReadingMode.Immediate
            }).ToMaybe();

            if (!definition.Any())
                throw new FailedToReadAssemblyDefinitionException(assembly);

            return definition.Single();
        }


        private void Weave(AssemblyDefinition definition)
        {
            _weaveSignal.Dispatch(definition);
        }


        /// <summary>
        /// It looks like Mono.Cecil (at least 0.9.5 and 0.9.6) don't implement a symbol writer
        /// for streams. We'll have to dump the contents to a temporary file and read that instead ;\
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="definition"></param>
        /// <returns></returns>
        private RawAssemblyData CreateRawAssemblyDataFromDefinition(ReeperAssembly assembly,
            AssemblyDefinition definition)
        {
            using (var assemblyFile = _tempFileFactory.Create())
            {
                var symbolPath = assemblyFile.Path + ".mdb";

                _log.Debug("Writing assembly to: " + assemblyFile.Path);

                definition.Write(assemblyFile.Path,
                    new WriterParameters {WriteSymbols = definition.MainModule.HasSymbols});

                if (definition.MainModule.HasSymbols && !File.Exists(symbolPath))
                    throw new FileNotFoundException("Symbol file was not created successfully",
                        Path.GetFileName(assemblyFile.Path + ".mdb"));

                var rawAssemblyStream = new MemoryStream(File.ReadAllBytes(assemblyFile.Path));
                var rawSymbolStream = File.Exists(symbolPath) ? new MemoryStream(File.ReadAllBytes(symbolPath)) : null;

                if (definition.MainModule.HasSymbols && File.Exists(symbolPath))
                    File.Delete(symbolPath);

                if (rawAssemblyStream.Length == 0)
                    throw new FailedToWriteDefinitionToStreamException(definition);

                if (definition.MainModule.HasSymbols && (rawSymbolStream == null || rawSymbolStream.Length == 0))
                    _log.Warning("Failed to write " + definition.MainModule.Name + " symbols to stream");

                return new RawAssemblyData(assembly, rawAssemblyStream,
                    rawSymbolStream.If(ms => assembly.Symbols.Any()).ToMaybe());              
            }
        }
    }
}
