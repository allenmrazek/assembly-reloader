using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ReloadableAddonLoader : IReloadableAddonLoader
    {
        private readonly ILog _log;
        private readonly IAddonSettings _settings;
        private readonly IGetAddonTypesForScene _getAddonTypesForScene;
        private readonly IMonoBehaviourFactory _mbFactory;
        private readonly List<Type> _loadedOnce = new List<Type>();

        public ReloadableAddonLoader(
            IAddonSettings settings,
            IGetAddonTypesForScene getAddonTypesForScene,
            IMonoBehaviourFactory mbFactory,
            [Name(LogKeys.AddonLoader)] ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (settings == null) throw new ArgumentNullException("settings");
            if (getAddonTypesForScene == null) throw new ArgumentNullException("getAddonTypesForScene");
            if (mbFactory == null) throw new ArgumentNullException("mbFactory");

            _log = log;
            _settings = settings;
            _getAddonTypesForScene = getAddonTypesForScene;
            _mbFactory = mbFactory;
            Handle = Maybe<ILoadedAssemblyHandle>.None;
        }


        public void CreateAddons(KSPAddon.Startup scene)
        {
            if (!Handle.Any())
            {
                _log.Warning("no handle found");
                return;
            }

            foreach (var addonType in GetAddonsToBeInitialized(scene))
            {
                var onceOnly = addonType.Value.once;

                if (!onceOnly || !_loadedOnce.Contains(addonType.Key))
                {
                    _log.Normal("Instantiating addon '{0}'", addonType.Key.Name);

                    try
                    {
                        _mbFactory.Create(addonType.Key);
                    }
                    catch (Exception e)
                    {
                        _log.Warning("Caught exception while creating " + addonType.Key.FullName);
                    }
                }
                else
                    _log.Debug("Skipping creation of " + addonType.Key.FullName + " because it has already been loaded.");
            }

        }


        private IEnumerable<KeyValuePair<Type, ReloadableAddonAttribute>> GetAddonsToBeInitialized(KSPAddon.Startup scene)
        {
            var forThisScene = _getAddonTypesForScene.Get(scene, Handle.Single());

            return !_settings.InstantlyAppliesToAllScenes ? 
                forThisScene.Union(_getAddonTypesForScene.Get(KSPAddon.Startup.Instantly, Handle.Single())) : 
                forThisScene;
        }


        public Maybe<ILoadedAssemblyHandle> Handle { get; set; }
    }
}
