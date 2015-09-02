extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ReloadableAddonLoader : IReloadableAddonLoader
    {
        private readonly ILog _log;
        private readonly IGetAddonTypesForScene _getAddonTypesForScene;
        private readonly IMonoBehaviourFactory _mbFactory;
        private readonly List<Type> _loadedOnce = new List<Type>();

        public ReloadableAddonLoader(
            IGetAddonTypesForScene getAddonTypesForScene,
            IMonoBehaviourFactory mbFactory,
            [Name(LogKey.AddonLoader)] ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (getAddonTypesForScene == null) throw new ArgumentNullException("getAddonTypesForScene");
            if (mbFactory == null) throw new ArgumentNullException("mbFactory");

            _log = log;
            _getAddonTypesForScene = getAddonTypesForScene;
            _mbFactory = mbFactory;
            Handle = Maybe<ILoadedAssemblyHandle>.None;
        }


        public void CreateAddons(KSP::KSPAddon.Startup scene)
        {
            if (!Handle.Any())
            {
                _log.Warning("no handle found");
                return;
            }

            foreach (var addonType in _getAddonTypesForScene.Get(scene, Handle.Single()))
            {
                var onceOnly = addonType.Attribute.once;

                if (!onceOnly || !_loadedOnce.Contains(addonType.Type))
                {
                    _log.Normal("Instantiating addon '{0}'", addonType.Type.Name);

                    try
                    {
                        _mbFactory.Create(addonType.Type);
                    }
                    catch (Exception e)
                    {
                        _log.Warning("Caught exception while creating " + addonType.Type.FullName);
                    }
                }
                else
                    _log.Debug("Skipping creation of " + addonType.Type.FullName + " because it has already been loaded.");
            }

        }


        public Maybe<ILoadedAssemblyHandle> Handle { get; set; }
    }
}
