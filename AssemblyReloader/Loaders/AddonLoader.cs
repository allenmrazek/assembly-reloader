using System;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game.Providers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders
{
    public class AddonLoader : IAddonLoader
    {
        private readonly ICurrentStartupSceneProvider _currentStartupScene;
        private readonly Func<bool> _alwaysLoadInstantAddons;

        public AddonLoader(
            [NotNull] ICurrentStartupSceneProvider currentStartupScene,
            [NotNull] Func<bool> alwaysLoadInstantAddons)
        {
            if (currentStartupScene == null) throw new ArgumentNullException("currentStartupScene");
            if (alwaysLoadInstantAddons == null) throw new ArgumentNullException("alwaysLoadInstantAddons");
            _currentStartupScene = currentStartupScene;
            _alwaysLoadInstantAddons = alwaysLoadInstantAddons;
        }


        public void Load([NotNull] Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var currentScene = _currentStartupScene.Get();

            LoadAddonsForScene(assembly, currentScene);

            if (currentScene != KSPAddon.Startup.Instantly && _alwaysLoadInstantAddons())
                LoadAddonsForScene(assembly, KSPAddon.Startup.Instantly);
        }


        private static void LoadAddonsForScene(Assembly assembly, KSPAddon.Startup scene)
        {
            
            var loadedAssembly = AssemblyLoader.loadedAssemblies.GetByAssembly(assembly);

            if (loadedAssembly.IsNull())
                throw new InvalidOperationException(assembly.FullName + " has not been loaded into KSP AssemblyLoader!");

            // this is pretty hacky but the goal here is to sneak addons that should be created
            // for this scene into AddonLoader's runOnce list without double-creating other addons
            //
            // todo: intercept calls to AssemblyLoader.loadedAssemblies and redirect them to a proxy object
            // which contains all of them in case client plugin is searching through that list
            var cache = AssemblyLoader.loadedAssemblies;

            try
            {
                AssemblyLoader.loadedAssemblies = new AssemblyLoader.LoadedAssembyList { loadedAssembly };
                global::AddonLoader.Instance.StartAddons(scene);
            }
            finally
            {
                AssemblyLoader.loadedAssemblies = cache;
            }
        }
    }
}
