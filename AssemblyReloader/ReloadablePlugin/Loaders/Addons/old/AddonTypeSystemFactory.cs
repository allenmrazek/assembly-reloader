//using System;
//using AssemblyReloader.CompositeRoot;
//using AssemblyReloader.Config;
//using AssemblyReloader.Game;
//using AssemblyReloader.Properties;
//using AssemblyReloader.StrangeIoC.extensions.implicitBind;
//using AssemblyReloader.Unsorted;

//namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
//{
//    [Implements(typeof(IAddonTypeSystemFactory))]
//// ReSharper disable once UnusedMember.Global
//    public class AddonTypeSystemFactory : IAddonTypeSystemFactory
//    {
//        private readonly IGameAssemblyLoader _gameAssemblyLoader;
//        private readonly IGameAddonLoader _gameAddonLoader;
//        private readonly IGetCurrentStartupScene _getStartupScene;
//        private readonly IGetTypesFromAssembly<KSPAddonType> _getAddonTypesFromAssembly;
//        private readonly IUnityObjectDestroyer _unityDestroyer;
//        private readonly IGetLoadedUnityComponents _getLoadedUnityComponents;

//        public AddonTypeSystemFactory(
//            [NotNull] IGameAssemblyLoader gameAssemblyLoader,
//            [NotNull] IGameAddonLoader gameAddonLoader, 
//            [NotNull] IGetCurrentStartupScene getStartupScene,
//            [NotNull] IGetTypesFromAssembly<KSPAddonType> getAddonTypesFromAssembly,
//            [NotNull] IUnityObjectDestroyer unityDestroyer, 
//            [NotNull] IGetLoadedUnityComponents getLoadedUnityComponents)
//        {
//            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
//            if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
//            if (getStartupScene == null) throw new ArgumentNullException("getStartupScene");
//            if (getAddonTypesFromAssembly == null) throw new ArgumentNullException("getAddonTypesFromAssembly");
//            if (unityDestroyer == null) throw new ArgumentNullException("unityDestroyer");
//            if (getLoadedUnityComponents == null) throw new ArgumentNullException("getLoadedUnityComponents");

//            _gameAssemblyLoader = gameAssemblyLoader;
//            _gameAddonLoader = gameAddonLoader;
//            _getStartupScene = getStartupScene;
//            _getAddonTypesFromAssembly = getAddonTypesFromAssembly;
//            _unityDestroyer = unityDestroyer;
//            _getLoadedUnityComponents = getLoadedUnityComponents;
//        }


//        private IReloadableAddonLoader CreateLoader([NotNull] PluginConfiguration pluginConfiguration)
//        {
//            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

//            return new ReloadableAddonLoader(_gameAssemblyLoader, _gameAddonLoader, _getStartupScene,
//                pluginConfiguration.InstantlyAppliesToAllScenes);
//        }


//        private IAddonUnloader CreateUnloader()
//        {
//            return new AddonUnloader(_getAddonTypesFromAssembly, _unityDestroyer, _getLoadedUnityComponents);
//        }


//        public IReloadableTypeSystem Create([NotNull] PluginConfiguration pluginConfiguration)
//        {
//            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");

//            return new AddonTypeSystem(CreateLoader(pluginConfiguration), CreateUnloader());
//        }
//    }
//}
