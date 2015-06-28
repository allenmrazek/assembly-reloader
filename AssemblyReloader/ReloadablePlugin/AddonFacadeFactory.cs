using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;

namespace AssemblyReloader.ReloadablePlugin
{
    public class AddonFacadeFactory : IAddonFacadeFactory
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IGameAddonLoader _gameAddonLoader;
        private readonly IGetCurrentStartupScene _getStartupScene;
        private readonly IGetTypesFromAssembly _getTypesFromAssembly;
        private readonly IUnityObjectDestroyer _unityDestroyer;
        private readonly IGetLoadedUnityComponents _getLoadedUnityComponents;

        public AddonFacadeFactory(
            [NotNull] IGameAssemblyLoader gameAssemblyLoader,
            [NotNull] IGameAddonLoader gameAddonLoader, 
            [NotNull] IGetCurrentStartupScene getStartupScene, 
            [NotNull] IGetTypesFromAssembly getTypesFromAssembly,
            [NotNull] IUnityObjectDestroyer unityDestroyer, 
            [NotNull] IGetLoadedUnityComponents getLoadedUnityComponents)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
            if (getStartupScene == null) throw new ArgumentNullException("getStartupScene");
            if (getTypesFromAssembly == null) throw new ArgumentNullException("getTypesFromAssembly");
            if (unityDestroyer == null) throw new ArgumentNullException("unityDestroyer");
            if (getLoadedUnityComponents == null) throw new ArgumentNullException("getLoadedUnityComponents");

            _gameAssemblyLoader = gameAssemblyLoader;
            _gameAddonLoader = gameAddonLoader;
            _getStartupScene = getStartupScene;
            _getTypesFromAssembly = getTypesFromAssembly;
            _unityDestroyer = unityDestroyer;
            _getLoadedUnityComponents = getLoadedUnityComponents;
        }


        private IAddonLoader CreateLoader([NotNull] PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

            return new AssemblyReloader.ReloadablePlugin.Loaders.Addons.AddonLoader(_gameAssemblyLoader, _gameAddonLoader, _getStartupScene,
                () => pluginConfiguration.InstantlyAppliesToEveryScene);
        }


        private IAddonUnloader CreateUnloader()
        {
            return new AddonUnloader(_getTypesFromAssembly, _unityDestroyer, _getLoadedUnityComponents);
        }


        public IReloadableObjectFacade Create([NotNull] PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

            return new AddonFacade(CreateLoader(pluginConfiguration), CreateUnloader());
        }
    }
}
