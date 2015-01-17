using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;
using AssemblyReloader.Factory;
using AssemblyReloader.Factory.Implementations;
using AssemblyReloader.Mediators;
using AssemblyReloader.Messages.Implementation;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.TypeTracking;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.Loaders.Addon
{
    class AddonLoader : ILoader, IAddonLoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        //private readonly List<AddonInfo> _addons;
        private readonly Dictionary<Type, AddonInfo> _addons;

        private readonly IDestructionMediator _destructionMediator;
        private readonly IMonoBehaviourFactory _addonFactory;
        private readonly IEnumerable<Type> _typesWhichAreAddons;
        private readonly ILog _log;
        private readonly StartupSceneFromGameSceneQuery _startupSceneFromGameSceneQuery;
        private readonly AddonAttributeFromTypeQuery _addonAttributeFromType;


        public AddonLoader(
            IEnumerable<Type> typesWhichAreAddons,
            IDestructionMediator destructionMediator,
            IMonoBehaviourFactory addonFactory,
            StartupSceneFromGameSceneQuery startupSceneFromGameSceneQuery,
            AddonAttributeFromTypeQuery addonAttributeFromType,
            ILog log)
        {
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (startupSceneFromGameSceneQuery == null)
                throw new ArgumentNullException("startupSceneFromGameSceneQuery");
            if (addonAttributeFromType == null) throw new ArgumentNullException("addonAttributeFromType");
            if (typesWhichAreAddons == null) throw new ArgumentNullException("typesWhichAreAddons");
            if (log == null) throw new ArgumentNullException("log");

            _destructionMediator = destructionMediator;
            _addonFactory = addonFactory;
            _startupSceneFromGameSceneQuery = startupSceneFromGameSceneQuery;
            _addonAttributeFromType = addonAttributeFromType;
            _typesWhichAreAddons = typesWhichAreAddons;
            _log = log;
            _addons = new Dictionary<Type, AddonInfo>();
        }




        public void Initialize()
        {
            _log.Debug("Initialize");

            if (_addons.Count > 0)
                throw new InvalidOperationException("AddonLoader was not deinitialized");

            foreach (var addonType in _typesWhichAreAddons)
            {
                var addonAttr = _addonAttributeFromType.GetKspAddonAttribute(addonType);

                if (!addonAttr.Any())
                    throw new InvalidOperationException(addonType.FullName + " does not have KSPAddon attribute");

                _addons.Add(addonType, new AddonInfo(addonType, addonAttr.Single()));
            }

            _log.Debug(_addons.Count + " total addons found");

        }



        public void Deinitialize()
        {
            _log.Debug("Deinitialize");
            DestroyLiveAddons();
            _addons.Clear();
        }



        private IEnumerable<KeyValuePair<Type, AddonInfo>> GetAddonsForScene(KSPAddon.Startup scene)
        {
            return ShouldBeCreated(_addons
                .Where(ty => ty.Value.addon.startup == scene || ty.Value.addon.startup == KSPAddon.Startup.Instantly));
        }



        private IEnumerable<KeyValuePair<Type, AddonInfo>> ShouldBeCreated(IEnumerable<KeyValuePair<Type, AddonInfo>> addonTypes)
        {
            return addonTypes
                .Where(kvp => (kvp.Value.addon.once && !kvp.Value.created) || !kvp.Value.addon.once);
        }




        private void CreateAddon(AddonInfo info)
        {
            var addon = new GameObject(info.type.Name);

            addon.AddComponent(info.type);
            
            info.created = true;
        }



        private void DestroyLiveAddons()
        {
            var instantiatedAddons = _addonFactory.GetLiveMonoBehaviours().Select(mb => mb.gameObject).ToList();

            _log.Verbose("Destroying " + instantiatedAddons.Count + " instantiated addons");
            instantiatedAddons.ForEach(go => _log.Normal("addon: " + (go.IsNull() ? "<null>" : go.name)));

            instantiatedAddons.ForEach(_destructionMediator.InformTargetOfDestruction);
            instantiatedAddons.ForEach(Object.Destroy);
        }


        public void LoadAddonsForScene(GameScenes scene)
        {
            _log.Debug("Loading addons for " + scene);

            var startupScene = _startupSceneFromGameSceneQuery.Query(scene);
            var addonsThatMatchScene = GetAddonsForScene(startupScene);
            var thatMatchScene = addonsThatMatchScene as IList<KeyValuePair<Type, AddonInfo>> ?? addonsThatMatchScene.ToList();

            _log.Verbose(thatMatchScene.Count + " addons eligible for instantiation");

            foreach (var addonType in thatMatchScene)
                _addonFactory.Create(addonType.Key, true);
            
        }


        private void OnSceneChanged(GameScenes newScene)
        {
            _log.Debug("OnSceneChanged");
            _addonFactory.RemoveDeadMonoBehaviours();
            LoadAddonsForScene(newScene);
        }



        public void Dispose()
        {
            DestroyLiveAddons();
            GC.SuppressFinalize(this);
        }
    }
}
