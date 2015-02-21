using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.PluginTracking;
using AssemblyReloader.Providers.SceneProviders;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.AddonLoader
{
    class AddonLoader : IAddonLoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true

        private readonly IAddonFactory _addonFactory;
        private readonly ILog _log;

        private readonly List<AddonInfo> _addons = new List<AddonInfo>();
        private readonly List<IDisposable> _created = new List<IDisposable>();


        public AddonLoader(
            IAddonFactory addonFactory,
            ILog log)
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (log == null) throw new ArgumentNullException("log");

            _addonFactory = addonFactory;
            _log = log;
        }



        private IEnumerable<AddonInfo> GetAddonsForStartup(KSPAddon.Startup startup)
        {
            return _addons
                .Where(info => info.Scene == startup)
                .Where(info => !info.RunOnce || (info.RunOnce && !info.created));
        }


        public void CreateForScene(KSPAddon.Startup scene)
        {
            _log.Debug("Loading addons for " + scene);

            var addonsThatMatchScene = GetAddonsForStartup(scene);

            var thatMatchScene = addonsThatMatchScene as AddonInfo[] ?? addonsThatMatchScene.ToArray();

            _log.Verbose(thatMatchScene.Count() + " addons eligible for instantiation");

            foreach (var addonType in thatMatchScene)
                _created.Add(_addonFactory.CreateAddon(addonType));
           
        }


        public void LoadAddonTypesFrom(Assembly assembly)
        {
            throw new NotImplementedException();
        }


        public void ClearAddonTypes(bool destroyLiveAddons)
        {
            if (destroyLiveAddons) DestroyLiveAddons();

            _addons.Clear();
        }


        public void DestroyLiveAddons()
        {
            _created.ForEach(c => c.Dispose());
            _created.Clear();
        }
    }
}
