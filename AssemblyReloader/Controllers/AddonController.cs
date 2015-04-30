using System;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Loaders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public class AddonController : IReloadableObjectController
    {
        private readonly IAddonLoader _loader;
        private readonly IAddonUnloader _unloader;

        public AddonController(
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
