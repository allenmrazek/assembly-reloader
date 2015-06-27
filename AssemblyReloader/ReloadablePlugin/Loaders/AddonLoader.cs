using System;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders
{
    public class AddonLoader : IAddonLoader
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IGameAddonLoader _gameAddonLoader;
        private readonly ICurrentStartupSceneProvider _currentStartupScene;
        private readonly Func<bool> _alwaysLoadInstantAddons;

        public AddonLoader(
            [NotNull] IGameAssemblyLoader gameAssemblyLoader, 
            [NotNull] IGameAddonLoader gameAddonLoader,
            [NotNull] ICurrentStartupSceneProvider currentStartupScene,
            [NotNull] Func<bool> alwaysLoadInstantAddons)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
            if (currentStartupScene == null) throw new ArgumentNullException("currentStartupScene");
            if (alwaysLoadInstantAddons == null) throw new ArgumentNullException("alwaysLoadInstantAddons");

            _gameAssemblyLoader = gameAssemblyLoader;
            _gameAddonLoader = gameAddonLoader;
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


        private void LoadAddonsForScene(Assembly assembly, KSPAddon.Startup scene)
        {
            
            //var loadedAssembly = AssemblyLoader.loadedAssemblies.GetByAssembly(assembly);
            var loadedAssembly = _gameAssemblyLoader.LoadedAssemblies.GetByAssembly(assembly);

            if (loadedAssembly.IsNull())
                throw new InvalidOperationException(assembly.FullName + " has not been loaded into KSP AssemblyLoader!");

            // this is pretty hacky but the goal here is to sneak addons that should be created
            // for this scene into AddonLoader's runOnce list without double-creating other addons
            //
            // todo: intercept calls to AssemblyLoader.loadedAssemblies and redirect them to a proxy object
            // which contains all of them in case client plugin is searching through that list
            var cache = _gameAssemblyLoader.LoadedAssemblies;

            try
            {
                //AssemblyLoader.loadedAssemblies = new AssemblyLoader.LoadedAssembyList { loadedAssembly };
                _gameAssemblyLoader.LoadedAssemblies = new global::AssemblyLoader.LoadedAssembyList {loadedAssembly};

                //global::AddonLoader.Instance.StartAddons(scene);
                _gameAddonLoader.StartAddons(scene);
            }
            finally
            {
                //AssemblyLoader.loadedAssemblies = cache;
                _gameAssemblyLoader.LoadedAssemblies = cache;
            }
        }
    }
}
