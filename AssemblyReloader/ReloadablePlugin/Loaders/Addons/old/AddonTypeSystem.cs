//using System;
//using AssemblyReloader.Game;
//using AssemblyReloader.Properties;

//namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
//{
//    public class AddonTypeSystem : IReloadableTypeSystem
//    {
//        private readonly IReloadableAddonLoader _loader;
//        private readonly IAddonUnloader _unloader;


//        public AddonTypeSystem(
//            [NotNull] IReloadableAddonLoader loader,
//            [NotNull] IAddonUnloader unloader)
//        {
//            if (loader == null) throw new ArgumentNullException("loader");
//            if (unloader == null) throw new ArgumentNullException("unloader");

//            _loader = loader;
//            _unloader = unloader;
//        }


//        public void CreateReloadableTypesFrom(ILoadedAssemblyHandle loadedAssembly)
//        {
//            if (loadedAssembly == null) throw new ArgumentNullException("loadedAssembly");

//            _loader.CreateAddons(loadedAssembly);
//        }


//        public void DestroyReloadableTypesFrom(ILoadedAssemblyHandle loadedAssembly)
//        {
//            if (loadedAssembly == null) throw new ArgumentNullException("loadedAssembly");

//            _unloader.DestroyAddons(loadedAssembly);
//        }
//    }
//}
