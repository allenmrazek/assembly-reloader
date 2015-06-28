using System;
using System.Linq;
using AssemblyReloader.DataObjects;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Generators;
using AssemblyReloader.Names;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Definition;
using AssemblyReloader.ReloadablePlugin.Definition.Operations;
using Mono.Cecil;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin
{
    [Implements(typeof(IAssemblyProviderFactory))]
// ReSharper disable once UnusedMember.Global
    public class AssemblyProviderFactory : IAssemblyProviderFactory
    {
        private readonly BaseAssemblyResolver _baseAssemblyResolver;
        private readonly IGetDebugSymbolsExistForDefinition _getDebugSymbolFileExists;
        private readonly ITemporaryFileFactory _temporaryFileFactory;
        private readonly IWeaveOperationFactory _weaveOperationFactory;
        private readonly IGetTypeDefinitions _getTypeDefinitions;
        private readonly IGetMethodDefinitions _getMethodDefinitions;
        private readonly Func<bool> _debugOutputToDisk;

        [Inject(LogNames.DefinitionWeaver)]
// ReSharper disable once MemberCanBePrivate.Global
        public ILog Log { get; set; }


        public AssemblyProviderFactory(
            [NotNull] BaseAssemblyResolver baseAssemblyResolver,
            [NotNull] IGetDebugSymbolsExistForDefinition getDebugSymbolFileExists,
            [NotNull] ITemporaryFileFactory temporaryFileFactory, 
            [NotNull] IWeaveOperationFactory weaveOperationFactory,
            [NotNull] IGetTypeDefinitions getTypeDefinitions,
            [NotNull] IGetMethodDefinitions getMethodDefinitions,
            [NotNull] Func<bool> debugOutputToDisk)
        {
            if (baseAssemblyResolver == null) throw new ArgumentNullException("baseAssemblyResolver");
            if (getDebugSymbolFileExists == null) throw new ArgumentNullException("getDebugSymbolFileExists");
            if (temporaryFileFactory == null) throw new ArgumentNullException("temporaryFileFactory");
            if (weaveOperationFactory == null) throw new ArgumentNullException("weaveOperationFactory");
            if (getTypeDefinitions == null) throw new ArgumentNullException("getTypeDefinitions");
            if (getMethodDefinitions == null) throw new ArgumentNullException("getMethodDefinitions");
            if (debugOutputToDisk == null) throw new ArgumentNullException("debugOutputToDisk");

            _baseAssemblyResolver = baseAssemblyResolver;
            _getDebugSymbolFileExists = getDebugSymbolFileExists;
            _temporaryFileFactory = temporaryFileFactory;
            _weaveOperationFactory = weaveOperationFactory;
            _getTypeDefinitions = getTypeDefinitions;
            _getMethodDefinitions = getMethodDefinitions;
            _debugOutputToDisk = debugOutputToDisk;
        }


        public IAssemblyProvider Create(PluginConfiguration pluginConfiguration)
        {
            var provider = new AssemblyProvider(
                new ConditionalWriteLoadedAssemblyToDisk(
                    new AssemblyDefinitionWeaver(
                        new AssemblyDefinitionFromDiskReader(_getDebugSymbolFileExists, _baseAssemblyResolver),
                        _getTypeDefinitions,
                        _getMethodDefinitions,
                        Log,
                        _weaveOperationFactory.Create(pluginConfiguration).ToArray()),

                    _debugOutputToDisk),
                _temporaryFileFactory, Log);

            return provider;
        }
    }
}
