using System;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    [Implements(typeof(ReloadablePluginFactory))]
    public class ReloadablePluginFactory
    {
        private readonly IPluginConfigurationProvider _configProvider;
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IAddonFacadeFactory _addonFacadeFactory;

        public ReloadablePluginFactory(
            [NotNull] IPluginConfigurationProvider configProvider,
            [NotNull] IGameAssemblyLoader gameAssemblyLoader, 
            [NotNull] IAddonFacadeFactory addonFacadeFactory )
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (addonFacadeFactory == null) throw new ArgumentNullException("addonFacadeFactory");

            _configProvider = configProvider;
            _gameAssemblyLoader = gameAssemblyLoader;
            _addonFacadeFactory = addonFacadeFactory;
        }


        IReloadablePlugin Create([NotNull] IFile location, [NotNull] IAssemblyProviderFactory assemblyProviderFactory)
        {
            if (location == null) throw new ArgumentNullException("location");
            if (assemblyProviderFactory == null) throw new ArgumentNullException("assemblyProviderFactory");

            var pluginConfiguration = _configProvider.Get(location);

            return new ReloadablePlugin(location, _gameAssemblyLoader,
                assemblyProviderFactory.Create(pluginConfiguration),
                _addonFacadeFactory.Create(pluginConfiguration));
        }
    }
}
