using System;
using System.Reflection;
using AssemblyReloader.Properties;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class AddonFacade : IReloadableObjectFacade
    {
        private readonly IAddonLoader _loader;
        private readonly IAddonUnloader _unloader;

        public AddonFacade(
            [NotNull] IAddonLoader loader,
            [NotNull] IAddonUnloader unloader
            )
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (unloader == null) throw new ArgumentNullException("unloader");

            _loader = loader;
            _unloader = unloader;
        }


        public void Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _loader.Load(assembly);
        }


        public void Unload(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _unloader.Unload(assembly);
        }
    }
}
