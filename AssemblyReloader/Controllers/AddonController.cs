using System;
using System.Reflection;
using AssemblyReloader.Destruction;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public class AddonController : IReloadableObjectController
    {
        private readonly IAddonLoader _addonLoader;
        private readonly IAddonDestroyer _addonDestroyer;
        private readonly ICurrentStartupSceneProvider _currentSceneProvider;

        public AddonController(
            IAddonLoader addonLoader,
            IAddonDestroyer addonDestroyer,
            ICurrentStartupSceneProvider currentSceneProvider
            )
        {
            if (addonLoader == null) throw new ArgumentNullException("addonLoader");
            if (addonDestroyer == null) throw new ArgumentNullException("addonDestroyer");
            if (currentSceneProvider == null) throw new ArgumentNullException("currentSceneProvider");

            _addonLoader = addonLoader;
            _addonDestroyer = addonDestroyer;
            _currentSceneProvider = currentSceneProvider;
        }


        public void Load(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _addonLoader.StartAddons(assembly, _currentSceneProvider.Get());
        }


        public void Unload(Assembly assembly, IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _addonDestroyer.DestroyAddonsFrom(assembly); 
        }
    }
}
