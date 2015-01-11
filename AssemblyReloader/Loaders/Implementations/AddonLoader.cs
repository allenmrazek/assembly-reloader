using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Factory;
using AssemblyReloader.Messages;
using AssemblyReloader.Messages.Implementation;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Loaders.Implementations
{
    class AddonLoader : ILoader, IAddonLoader, IConsumer<AddonCreated>, IConsumer<AddonDestroyed>
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        private readonly List<AddonInfo> _addons;

        private readonly IChannel _messageChannel;
        private readonly ReloadableAssembly _assembly;
        private readonly AddonInfoFactory _infoFactory;
        private readonly CommandFactory _commandFactory;
        private readonly AddonsFromAssemblyQuery _getAddonsFromAssembly;
        private readonly CurrentStartupSceneProvider _currentStartupSceneProvider;
        private readonly Log _log;

        private readonly List<GameObject> _created;



        public AddonLoader(
            IChannel messageChannel,
            ReloadableAssembly assembly,
            AddonInfoFactory infoFactory,
            CommandFactory commandFactory,
            AddonsFromAssemblyQuery getAddonsFromAssembly,
            CurrentStartupSceneProvider currentStartupSceneProvider,
            Log log)
        {
            if (messageChannel == null) throw new ArgumentNullException("messageChannel");
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");
            if (commandFactory == null) throw new ArgumentNullException("commandFactory");
            if (getAddonsFromAssembly == null) throw new ArgumentNullException("getAddonsFromAssembly");
            if (currentStartupSceneProvider == null) throw new ArgumentNullException("currentStartupSceneProvider");
            if (log == null) throw new ArgumentNullException("log");

            _messageChannel = messageChannel;
            _assembly = assembly;
            _infoFactory = infoFactory;
            _commandFactory = commandFactory;
            _getAddonsFromAssembly = getAddonsFromAssembly;
            _currentStartupSceneProvider = currentStartupSceneProvider;
            _log = log;
            _addons = new List<AddonInfo>();
            _created = new List<GameObject>();
        }



        public void Initialize()
        {
            foreach (var addon in _getAddonsFromAssembly.Get(_assembly.Loaded))
                _addons.Add(_infoFactory.Create(addon, _assembly));

            DoLevelLoad(_currentStartupSceneProvider.Get());
        }



        public void DoLevelLoad(KSPAddon.Startup scene)
        {
            var addons = GetAddonsForScene(scene)
                .Select(ty => _addons.Single(t => t.type == ty))
                .ToList();

            // exclude any that are marked runOnce and were already created previously
            addons.RemoveAll(ai => ai.RunOnce && ai.created);

            addons.ForEach(CreateAddon);
            
            
        }



        private IEnumerable<Type> GetAddonsForScene(KSPAddon.Startup scene)
        {
            return
                _getAddonsFromAssembly.Get(_assembly.Loaded)
                    .Where(type =>
                    {
                        var addon = _getAddonsFromAssembly.GetKSPAddonFromType(type);

                        return addon.Any() &&
                               addon.First().startup == scene;
                    })
                    .ToList();
        }



        private void CreateAddon(AddonInfo info)
        {
            _log.Verbose("AddonLoader: Creating KSPAddon '{0}'", info.type.FullName);

            var addon = new GameObject(info.type.Name);

            addon.AddComponent(info.type);
            
            info.created = true;

  
        }



        public void StartTrackingAddon(GameObject go)
        {
            if (_created.Contains(go))
                throw new ArgumentException(go.name + " is already being tracked");

            _log.Verbose("AddonLoader: started tracking " + go.name);

            _created.Add(go);
        }



        public void StopTrackingAddon(GameObject go)
        {
            if (!_created.Contains(go))
                throw new ArgumentException(go.name + " is not being tracked");

            _log.Verbose("AddonLoader: stopped tracking " + go.name);

            _created.Remove(go);
        }



        public void Consume(AddonCreated message)
        {
            StartTrackingAddon(message.Addon);
        }



        public void Consume(AddonDestroyed message)
        {
            StopTrackingAddon(message.Addon);
        }
    }
}
