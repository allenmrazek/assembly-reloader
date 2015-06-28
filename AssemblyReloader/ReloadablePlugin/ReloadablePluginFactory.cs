using System;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    [Implements(typeof(ReloadablePluginFactory))]
    public class ReloadablePluginFactory
    {
        private readonly IAssemblyProviderFactory _assemblyProviderFactory;
        private readonly IPluginConfigurationProvider _configProvider;
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IAddonFacadeFactory _addonFacadeFactory;

        public ReloadablePluginFactory(
            [NotNull] IAssemblyProviderFactory assemblyProviderFactory,
            [NotNull] IPluginConfigurationProvider configProvider,
            [NotNull] IGameAssemblyLoader gameAssemblyLoader, 
            [NotNull] IAddonFacadeFactory addonFacadeFactory )
        {
            if (assemblyProviderFactory == null) throw new ArgumentNullException("assemblyProviderFactory");
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (addonFacadeFactory == null) throw new ArgumentNullException("addonFacadeFactory");

            _assemblyProviderFactory = assemblyProviderFactory;
            _configProvider = configProvider;
            _gameAssemblyLoader = gameAssemblyLoader;
            _addonFacadeFactory = addonFacadeFactory;
        }


        public IReloadablePlugin Create([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            var pluginConfiguration = _configProvider.Get(location);

            return new ReloadablePlugin(location, _gameAssemblyLoader,
                _assemblyProviderFactory.Create(pluginConfiguration, location.Directory),
                _addonFacadeFactory.Create(pluginConfiguration));
        }
    }
}
