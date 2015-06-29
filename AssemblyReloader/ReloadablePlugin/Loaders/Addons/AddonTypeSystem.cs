using System;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class AddonTypeSystem : IReloadableTypeSystem
    {
        private readonly IAddonLoader _loader;
        private readonly IAddonUnloader _unloader;
        private readonly IGetTypesFromAssembly<AddonType> _addonTypesFromAssembly;


        public AddonTypeSystem(
            [NotNull] IAddonLoader loader,
            [NotNull] IAddonUnloader unloader,
            [NotNull] IGetTypesFromAssembly<AddonType> addonTypesFromAssembly
            )
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (unloader == null) throw new ArgumentNullException("unloader");
            if (addonTypesFromAssembly == null) throw new ArgumentNullException("addonTypesFromAssembly");

            _loader = loader;
            _unloader = unloader;
            _addonTypesFromAssembly = addonTypesFromAssembly;
        }


        //public void CreateReloadableTypesFrom(Assembly assembly, IFile location)
        //{
        //    if (assembly == null) throw new ArgumentNullException("assembly");

        //    _loader.Load(assembly);
        //}


        //public void DestroyReloadableTypesFrom(Assembly assembly, IFile location)
        //{
        //    if (assembly == null) throw new ArgumentNullException("assembly");

        //    _unloader.Unload(assembly);
        //}
        public void CreateReloadableTypesFrom(ILoadedAssemblyHandle assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            throw new NotImplementedException();
        }

        public void DestroyReloadableTypesFrom(ILoadedAssemblyHandle assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            throw new NotImplementedException();
        }
    }
}
