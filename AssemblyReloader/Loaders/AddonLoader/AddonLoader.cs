using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.PluginTracking;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.AddonLoader
{
    class AddonLoader : IAddonLoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        private readonly IAddonFactory _addonFactory;
        private readonly ITypesFromAssemblyQuery _addonsFromAssemblyQuery;
        private readonly ICurrentStartupSceneProvider _currentScene;
        private readonly ILog _log;

        private List<AddonInfo> _addons = new List<AddonInfo>();
        private readonly List<IDisposable> _created = new List<IDisposable>();


        public AddonLoader(
            IAddonFactory addonFactory,
            ITypesFromAssemblyQuery addonsFromAssemblyQuery,
            ICurrentStartupSceneProvider currentScene,
            ILog log)
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");
            if (currentScene == null) throw new ArgumentNullException("currentScene");
            if (log == null) throw new ArgumentNullException("log");

            _addonFactory = addonFactory;
            _addonsFromAssemblyQuery = addonsFromAssemblyQuery;
            _currentScene = currentScene;
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


        public void LoadAddonTypes(Assembly assembly)
        {
            _addons = _addonsFromAssemblyQuery.Get(assembly).Select(ty => _addonFactory.CreateInfoForAddonType(ty)).ToList();
            CreateForScene(_currentScene.Get());
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
