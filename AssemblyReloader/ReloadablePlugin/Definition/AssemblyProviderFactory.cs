using System;
using System.Linq;
using AssemblyReloader.DataObjects;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Definition.Operations;
using Mono.Cecil;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Definition
{
// ReSharper disable once UnusedMember.Global
    public class AssemblyProviderFactory : IAssemblyProviderFactory
    {
        private readonly BaseAssemblyResolver _baseAssemblyResolver;
        private readonly IGetDebugSymbolsExistForDefinition _getDebugSymbolFileExists;
        private readonly IWeaveOperationFactory _weaveOperationFactory;
        private readonly IGetTypeDefinitions _getTypeDefinitions;
        private readonly IGetMethodDefinitions _getMethodDefinitions;
        private readonly ILog _log;
        private readonly Func<bool> _debugOutputToDisk;

        public AssemblyProviderFactory(
            [NotNull] BaseAssemblyResolver baseAssemblyResolver,
            [NotNull] IGetDebugSymbolsExistForDefinition getDebugSymbolFileExists,
            [NotNull] IWeaveOperationFactory weaveOperationFactory,
            [NotNull] IGetTypeDefinitions getTypeDefinitions,
            [NotNull] IGetMethodDefinitions getMethodDefinitions,
            [NotNull] ILog log,
            [NotNull] Func<bool> debugOutputToDisk)
        {
            if (baseAssemblyResolver == null) throw new ArgumentNullException("baseAssemblyResolver");
            if (getDebugSymbolFileExists == null) throw new ArgumentNullException("getDebugSymbolFileExists");
            if (weaveOperationFactory == null) throw new ArgumentNullException("weaveOperationFactory");
            if (getTypeDefinitions == null) throw new ArgumentNullException("getTypeDefinitions");
            if (getMethodDefinitions == null) throw new ArgumentNullException("getMethodDefinitions");
            if (log == null) throw new ArgumentNullException("log");
            if (debugOutputToDisk == null) throw new ArgumentNullException("debugOutputToDisk");

            _baseAssemblyResolver = baseAssemblyResolver;
            _getDebugSymbolFileExists = getDebugSymbolFileExists;
            _weaveOperationFactory = weaveOperationFactory;
            _getTypeDefinitions = getTypeDefinitions;
            _getMethodDefinitions = getMethodDefinitions;
            _log = log;
            _debugOutputToDisk = debugOutputToDisk;
        }


        public IAssemblyProvider Create(PluginConfiguration pluginConfiguration, IDirectory temporaryDirectory)
        {
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");
            if (temporaryDirectory == null) throw new ArgumentNullException("temporaryDirectory");

            var provider = new AssemblyProvider(
                new ConditionalWriteLoadedAssemblyToDisk(
                    new AssemblyDefinitionWeaver(
                        new AssemblyDefinitionFromDiskReader(_getDebugSymbolFileExists, _baseAssemblyResolver, _log),
                        _getTypeDefinitions,
                        _getMethodDefinitions,
                        _log,
                        _weaveOperationFactory.Create(pluginConfiguration).ToArray()),

                    _debugOutputToDisk),
                new TemporaryFileFactory(temporaryDirectory, new RandomStringGenerator()), _log);

            return provider;
        }
    }
}
