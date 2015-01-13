using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Mediators;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
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

        private readonly ReloadableAssembly _assembly;
        private readonly IDestructionMediator _destructionMediator;
        private readonly AddonsFromAssemblyQuery _getAddonsFromAssembly;
        private readonly CurrentStartupSceneProvider _currentStartupSceneProvider;
        private readonly Log _log;

        private readonly List<GameObject> _created;



        public AddonLoader(
            ReloadableAssembly assembly,
            IDestructionMediator destructionMediator,
            AddonsFromAssemblyQuery getAddonsFromAssembly,
            CurrentStartupSceneProvider currentStartupSceneProvider,
            Log log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (getAddonsFromAssembly == null) throw new ArgumentNullException("getAddonsFromAssembly");
            if (currentStartupSceneProvider == null) throw new ArgumentNullException("currentStartupSceneProvider");
            if (log == null) throw new ArgumentNullException("log");

            _assembly = assembly;
            _destructionMediator = destructionMediator;
            _getAddonsFromAssembly = getAddonsFromAssembly;
            _currentStartupSceneProvider = currentStartupSceneProvider;
            _log = log;
            _addons = new List<AddonInfo>();
            _created = new List<GameObject>();
        }



        ~AddonLoader()
        {
            var instantiatedAddons = _getAddonsFromAssembly.Get(_assembly.Loaded)
                .SelectMany(Object.FindObjectsOfType)
                .Select(obj => obj as GameObject)
                .ToList();

            _log.Verbose("AddonLoader: Destroying " + instantiatedAddons.Count + " instantiated addons from " +
                         _assembly.Loaded.FullName);

            instantiatedAddons.ForEach(_destructionMediator.InformTargetOfDestruction);
            instantiatedAddons.ForEach(Object.Destroy);
        }



        public void Initialize()
        {
            foreach (var addonType in _getAddonsFromAssembly.Get(_assembly.Loaded))
            {
                var kspAddon = _getAddonsFromAssembly.GetKSPAddonFromType(addonType);

                if (!kspAddon.Any())
                    throw new InvalidOperationException(addonType.FullName + " does not have KSPAddon Attribute");

                _addons.Add(new AddonInfo(addonType, kspAddon.First()));
            }

            DoLevelLoad(_currentStartupSceneProvider.Get());
        }



        public void DoLevelLoad(KSPAddon.Startup scene)
        {
            _log.Verbose("AddonLoader: handling level load for " + scene.ToString());

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
    }
}
