using System;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;

namespace AssemblyReloader.ReloadablePlugin
{
    [Implements(typeof(IAddonFacadeFactory))]
// ReSharper disable once UnusedMember.Global
    public class AddonFacadeFactory : IAddonFacadeFactory
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IGameAddonLoader _gameAddonLoader;
        private readonly IGetCurrentStartupScene _getStartupScene;
        private readonly IGetTypesFromAssembly<AddonType> _getAddonTypesFromAssembly;
        private readonly IUnityObjectDestroyer _unityDestroyer;
        private readonly IGetLoadedUnityComponents _getLoadedUnityComponents;

        public AddonFacadeFactory(
            [NotNull] IGameAssemblyLoader gameAssemblyLoader,
            [NotNull] IGameAddonLoader gameAddonLoader, 
            [NotNull] IGetCurrentStartupScene getStartupScene,
            [NotNull] IGetTypesFromAssembly<AddonType> getAddonTypesFromAssembly,
            [NotNull] IUnityObjectDestroyer unityDestroyer, 
            [NotNull] IGetLoadedUnityComponents getLoadedUnityComponents)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
            if (getStartupScene == null) throw new ArgumentNullException("getStartupScene");
            if (getAddonTypesFromAssembly == null) throw new ArgumentNullException("getAddonTypesFromAssembly");
            if (unityDestroyer == null) throw new ArgumentNullException("unityDestroyer");
            if (getLoadedUnityComponents == null) throw new ArgumentNullException("getLoadedUnityComponents");

            _gameAssemblyLoader = gameAssemblyLoader;
            _gameAddonLoader = gameAddonLoader;
            _getStartupScene = getStartupScene;
            _getAddonTypesFromAssembly = getAddonTypesFromAssembly;
            _unityDestroyer = unityDestroyer;
            _getLoadedUnityComponents = getLoadedUnityComponents;
        }


        private IAddonLoader CreateLoader([NotNull] PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

            return new Loaders.Addons.AddonLoader(_gameAssemblyLoader, _gameAddonLoader, _getStartupScene,
                () => pluginConfiguration.InstantlyAppliesToEveryScene);
        }


        private IAddonUnloader CreateUnloader()
        {
            return new AddonUnloader(_getAddonTypesFromAssembly, _unityDestroyer, _getLoadedUnityComponents);
        }


        public IReloadableObjectFacade Create([NotNull] PluginConfiguration pluginConfiguration)
        {
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

            return new AddonFacade(CreateLoader(pluginConfiguration), CreateUnloader());
        }
    }
}
