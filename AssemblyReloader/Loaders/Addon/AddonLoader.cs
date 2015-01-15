﻿using System;
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
        private readonly List<AddonInfo> _addons;

        private readonly IDestructionMediator _destructionMediator;
        private readonly QueryProvider _queryProvider;
        private readonly IEnumerable<Type> _typesWhichAreAddons;
        private readonly ILog _log;
        private readonly IGameEventSubscription _eventSubscription;



        public AddonLoader(
            IEnumerable<Type> typesWhichAreAddons,
            IGameEventSource<LevelWasLoadedDelegate> levelLoadedEvent,
            IDestructionMediator destructionMediator,
            QueryProvider queryProvider,
            ILog log)
        {
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");
            if (typesWhichAreAddons == null) throw new ArgumentNullException("typesWhichAreAddons");
            if (levelLoadedEvent == null) throw new ArgumentNullException("levelLoadedEvent");
            if (log == null) throw new ArgumentNullException("log");

            _destructionMediator = destructionMediator;
            _queryProvider = queryProvider;
            _typesWhichAreAddons = typesWhichAreAddons;
            _log = log;
            _addons = new List<AddonInfo>();
            _eventSubscription = levelLoadedEvent.Add(OnSceneChanged);
        }



        public void Consume(SceneChange message)
        {
            _log.Verbose("handling level load for " + message.Scene.ToString());

            var startupScene = _queryProvider.GetStartupSceneFromGameSceneQuery().Query(message.Scene);

            var addons = GetAddonsForScene(startupScene)
                .Select(ty => _addons.Single(t => t.type == ty))
                .ToList();

            // exclude any that are marked runOnce and were already created previously
            addons.RemoveAll(ai => ai.RunOnce && ai.created);

            addons.ForEach(CreateAddon);
        }



        ~AddonLoader()
        {
            Dispose();
        }



        public void Dispose()
        {
            _eventSubscription.Dispose();

            var instantiatedAddons = _typesWhichAreAddons
                .SelectMany(Object.FindObjectsOfType)
                .Select(obj => obj as GameObject)
                .ToList();

            _log.Verbose("Destroying " + instantiatedAddons.Count + " instantiated addons");

            instantiatedAddons.ForEach(_destructionMediator.InformTargetOfDestruction);
            instantiatedAddons.ForEach(Object.Destroy);

            instantiatedAddons.Clear();

            GC.SuppressFinalize(this);
        }



        private IEnumerable<Type> GetAddonsForScene(KSPAddon.Startup scene)
        {
            return
                _typesWhichAreAddons
                    .Where(type =>
                    {
                        var addon = _queryProvider.GetAddonAttributeQuery().GetKspAddonAttribute(type);

                        return addon.Any() &&
                               addon.First().startup == scene;
                    })
                    .ToList();
        }



        private void CreateAddon(AddonInfo info)
        {
            _log.Verbose("Creating KSPAddon '{0}'", info.type.FullName);

            var addon = new GameObject(info.type.Name);

            addon.AddComponent(info.type);
            
            info.created = true;
        }



        public void Initialize()
        {
            _log.Debug("Initialize");
        }



        public void Deinitialize()
        {
            _log.Debug("Deinitialize");
        }



        private void OnSceneChanged(GameScenes newScene)
        {
            _log.Debug("OnSceneChanged to " + newScene);
        }
    }
}
