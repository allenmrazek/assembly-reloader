using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class ReloadableAddonLoader : IReloadableAddonLoader
    {
        private readonly ILog _log;
        private readonly IAddonSettings _settings;
        private readonly Dictionary<Type, bool> _loadedOnce = new Dictionary<Type, bool>();

        private Maybe<ILoadedAssemblyHandle> _handle = Maybe<ILoadedAssemblyHandle>.None;

        private AddonLoader loader;

        public ReloadableAddonLoader(
            ILog log,
            IAddonSettings settings)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (settings == null) throw new ArgumentNullException("settings");

            _log = log;
            _settings = settings;

            loader = AddonLoader.Instance;
        }





        //public void CreateAddons([NotNull] ILoadedAssemblyHandle assemblyHandle)
        //{
        //    if (assemblyHandle == null) throw new ArgumentNullException("assemblyHandle");

        //    var currentScene = _getCurrentStartupScene.Get();

        //    LoadAddonsForScene(assemblyHandle.LoadedAssembly.assembly, currentScene);

        //    if (currentScene != KSPAddon.Startup.Instantly && _settings.InstantlyAppliesToAllScenes)
        //        LoadAddonsForScene(assemblyHandle.LoadedAssembly.assembly, KSPAddon.Startup.Instantly);
        //}

        public void CreateAddons(KSPAddon.Startup scene)
        {
            if (!Handle.Any())
            {
                _log.Warning("AddonLoader: no handle found");
                return;
            }

            // todo: scan loaded assembly handle for ReloadableAddonAttributes and create them
            _log.Normal("Here I would create addons for " + scene + " for " +
                        Handle.Single().LoadedAssembly.name);

            var valid = Handle.Single().LoadedAssembly.assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (MonoBehaviour)))
                .Where(
                    t =>
                        t.GetCustomAttributes(typeof (ReloadableAddonAttribute), true)
                            .Any(attr => ((ReloadableAddonAttribute) attr).startup == scene))
                .ToList();

            _log.Normal("These addons would be created now:");
            valid.ForEach(addon => _log.Normal("Addon: " + addon.FullName));
            _log.Normal("(end list)");

            AddonLoader.Instance.gameObject.PrintComponents(_log);
            if (loader == null)
                _log.Warning("AddonLoader was destroyed");
            loader = AddonLoader.Instance;
        }


        public Maybe<ILoadedAssemblyHandle> Handle
        {
            get
            {
                return _handle;
            }
            set
            {
                // todo: destroy old addons?
                _handle = value;
            }
        }
    }
}
