﻿using System;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using ReeperCommon.Extensions;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class AddonLoader : IAddonLoader
    {
        private readonly IGameAssemblyLoader _gameAssemblyLoader;
        private readonly IGameAddonLoader _gameAddonLoader;
        private readonly IGetCurrentStartupScene _getCurrentStartupScene;
        private readonly Func<bool> _alwaysLoadInstantAddons;

        public AddonLoader(
            [NotNull] IGameAssemblyLoader gameAssemblyLoader, 
            [NotNull] IGameAddonLoader gameAddonLoader,
            [NotNull] IGetCurrentStartupScene getCurrentStartupScene,
            [NotNull] Func<bool> alwaysLoadInstantAddons)
        {
            if (gameAssemblyLoader == null) throw new ArgumentNullException("gameAssemblyLoader");
            if (gameAddonLoader == null) throw new ArgumentNullException("gameAddonLoader");
            if (getCurrentStartupScene == null) throw new ArgumentNullException("getCurrentStartupScene");
            if (alwaysLoadInstantAddons == null) throw new ArgumentNullException("alwaysLoadInstantAddons");

            _gameAssemblyLoader = gameAssemblyLoader;
            _gameAddonLoader = gameAddonLoader;
            _getCurrentStartupScene = getCurrentStartupScene;
            _alwaysLoadInstantAddons = alwaysLoadInstantAddons;
        }


        public void Load([NotNull] Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            var currentScene = _getCurrentStartupScene.Get();

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
