using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;
using AssemblyReloader.Factory;
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
    class AddonLoader : ILoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        //private readonly List<AddonInfo> _addons;
        private readonly Dictionary<Type, AddonInfo> _addons;

        private readonly IDestructionMediator _destructionMediator;
        private readonly IEnumerable<Type> _typesWhichAreAddons;
        private readonly ILog _log;
        private readonly IGameEventSubscription _eventSubscription;
        private readonly StartupSceneFromGameSceneQuery _startupSceneFromGameSceneQuery;
        private readonly AddonAttributeFromTypeQuery _addonAttributeFromType;


        public AddonLoader(
            IEnumerable<Type> typesWhichAreAddons,
            IGameEventSource<LevelWasLoadedDelegate> levelLoadedEvent,
            IDestructionMediator destructionMediator,
            StartupSceneFromGameSceneQuery startupSceneFromGameSceneQuery,
            AddonAttributeFromTypeQuery addonAttributeFromType,
            ILog log)
        {
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (startupSceneFromGameSceneQuery == null)
                throw new ArgumentNullException("startupSceneFromGameSceneQuery");
            if (addonAttributeFromType == null) throw new ArgumentNullException("addonAttributeFromType");
            if (typesWhichAreAddons == null) throw new ArgumentNullException("typesWhichAreAddons");
            if (levelLoadedEvent == null) throw new ArgumentNullException("levelLoadedEvent");
            if (log == null) throw new ArgumentNullException("log");

            _destructionMediator = destructionMediator;
            _startupSceneFromGameSceneQuery = startupSceneFromGameSceneQuery;
            _addonAttributeFromType = addonAttributeFromType;
            _typesWhichAreAddons = typesWhichAreAddons;
            _log = log;
            _addons = new Dictionary<Type, AddonInfo>();

            _eventSubscription = levelLoadedEvent.Add(OnSceneChanged);
        }



        //public void Consume(SceneChange message)
        //{
        //    _log.Verbose("handling level load for " + message.Scene.ToString());

        //    var startupScene = _startupSceneFromGameSceneQuery.Query(message.Scene);

        //    var addons = GetAddonsForScene(startupScene)
        //        .Select(ty => _addons.Single(t => t.type == ty))
        //        .ToList();

        //    // exclude any that are marked runOnce and were already created previously
        //    addons.RemoveAll(ai => ai.RunOnce && ai.created);

        //    addons.ForEach(CreateAddon);
        //}



        ~AddonLoader()
        {
            Dispose();
        }



        public void Dispose()
        {
            DestroyLiveAddons();

            _eventSubscription.Dispose();
            
            GC.SuppressFinalize(this);
        }



        private IEnumerable<KeyValuePair<Type, AddonInfo>> GetAddonsForScene(KSPAddon.Startup scene)
        {
            return ShouldBeCreated(_addons
                .Where(ty => ty.Value.addon.startup == scene));
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
            var instantiatedAddons = _typesWhichAreAddons
                .SelectMany(Object.FindObjectsOfType)
                .Select(obj => obj as GameObject)
                .ToList();

            _log.Verbose("Destroying " + instantiatedAddons.Count + " instantiated addons");

            instantiatedAddons.ForEach(_destructionMediator.InformTargetOfDestruction);
            instantiatedAddons.ForEach(Object.Destroy);
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

            _log.Debug(_addons.Count + " found");
        }



        public void Deinitialize()
        {
            _log.Debug("Deinitialize");
            _addons.Clear();
            DestroyLiveAddons();
        }



        private void OnSceneChanged(GameScenes newScene)
        {
            _log.Debug("OnSceneChanged to " + newScene);

            _log.Debug("querying");
            if (_startupSceneFromGameSceneQuery.IsNull())
                _log.Error("is null");
            var startupScene = _startupSceneFromGameSceneQuery.Query(newScene);

            _log.Debug("query2");
            var addonsThatMatchScene = GetAddonsForScene(startupScene);

            _log.Debug("output");
            var thatMatchScene = addonsThatMatchScene as IList<KeyValuePair<Type, AddonInfo>> ?? addonsThatMatchScene.ToList();

            _log.Verbose(thatMatchScene.Count + " addons eligible");

            foreach (var addonType in thatMatchScene)
            {
                _log.Verbose("Creating addon " + addonType.Key.FullName);
                CreateAddon(addonType.Value);
            }

        }
    }
}
