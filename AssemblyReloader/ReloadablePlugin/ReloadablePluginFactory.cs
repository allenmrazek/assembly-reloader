using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Game;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    [Implements(typeof(ReloadablePluginFactory))]
    public class ReloadablePluginFactory
    {
        private readonly IPluginConfigurationProvider _configProvider;
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IAssemblyProvider _assemblyProvider;
        private readonly IAddonFacadeFactory _addonFacadeFactory;

        public ReloadablePluginFactory(
            [NotNull] IPluginConfigurationProvider configProvider,
            [NotNull] IGameAssemblyLoader gameAssemblyLoader, 
            [NotNull] IAssemblyProvider assemblyProvider, 
            [NotNull] IAddonFacadeFactory addonFacadeFactory )
        {
            if (configProvider == null) throw new ArgumentNullException("configProvider");
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (assemblyProvider == null) throw new ArgumentNullException("assemblyProvider");
            if (addonFacadeFactory == null) throw new ArgumentNullException("addonFacadeFactory");

            _configProvider = configProvider;
            _gameAssemblyLoader = gameAssemblyLoader;
            _assemblyProvider = assemblyProvider;
            _addonFacadeFactory = addonFacadeFactory;
        }


        IReloadablePlugin Create(IFile location)
        {
            var pluginConfiguration = _configProvider.Get(location);

            return new ReloadablePlugin(location, _gameAssemblyLoader,
                _assemblyProvider,
                _addonFacadeFactory.Create(pluginConfiguration));
        }
    }
}
