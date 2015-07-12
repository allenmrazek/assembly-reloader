using System;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Definition;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
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
        private readonly IAddonTypeSystemFactory _addonTypeSystemFactory;

        public ReloadablePluginFactory(
            [NotNull] IAssemblyProviderFactory assemblyProviderFactory,
            [NotNull] IPluginConfigurationProvider configProvider,
            [NotNull] IGameAssemblyLoader gameAssemblyLoader, 
            [NotNull] IAddonTypeSystemFactory addonTypeSystemFactory )
        {
            if (assemblyProviderFactory == null) throw new ArgumentNullException("assemblyProviderFactory");
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (addonTypeSystemFactory == null) throw new ArgumentNullException("addonTypeSystemFactory");

            _assemblyProviderFactory = assemblyProviderFactory;
            _configProvider = configProvider;
            _gameAssemblyLoader = gameAssemblyLoader;
            _addonTypeSystemFactory = addonTypeSystemFactory;
        }


        public IReloadablePlugin Create([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");

            var pluginConfiguration = _configProvider.Get(location);

            return new ReloadablePlugin(location, _gameAssemblyLoader,
                _assemblyProviderFactory.Create(pluginConfiguration, location.Directory),
                _addonTypeSystemFactory.Create(pluginConfiguration));
        }
    }
}
