using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Factory;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Loaders
{
    class KspAddonLoader : ILoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        private readonly List<AddonInfo> _addons;

        private readonly ReloadableAssembly _assembly;
        private readonly AddonInfoFactory _addonFactory;
        private readonly KspAddonsFromAssemblyQuery _provider;
        private readonly Log _log;


        public KspAddonLoader(
            ReloadableAssembly assembly,
            AddonInfoFactory addonFactory,
            KspAddonsFromAssemblyQuery provider,
            KspCurrentStartupSceneProvider currentStartupSceneProvider,
            Log log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (provider == null) throw new ArgumentNullException("provider");
            if (currentStartupSceneProvider == null) throw new ArgumentNullException("currentStartupSceneProvider");
            if (log == null) throw new ArgumentNullException("log");

            _assembly = assembly;
            _addonFactory = addonFactory;
            _provider = provider;
            _log = log;
            _addons = new List<AddonInfo>();

            foreach (var addon in provider.Get(_assembly.Loaded))
                _addons.Add(_addonFactory.Create(addon, assembly));

            DoLevelLoad(currentStartupSceneProvider.Get());
        }



        ~KspAddonLoader()
        {
            // todo: unload all loaded addons
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
                _provider.Get(_assembly.Loaded)
                    .Where(type =>
                    {
                        var addon = _provider.GetKSPAddonFromType(type);

                        return addon.Any() &&
                               addon.First().startup == scene;
                    })
                    .ToList();
        }



        private void CreateAddon(AddonInfo info)
        {
            _log.Verbose("AddonLoader: Creating KSPAddon '{0}'", info.type.FullName);

            var addon = new GameObject(info.type.FullName);

            addon.AddComponent(info.type);

            info.created = true;
        }


        //    for (int i = 0; i < addons.Count; ++i)
        //    {
        //        var addon = addons[i];

        //        if (addon.created && addon.RunOnce)
        //            continue; // this addon was already loaded

        //        // should this addon be initialized in current scene?
        //        if ((addon.Scenes & mask) != 0)
        //        {
        //            GameObject go = new GameObject(addon.type.Name);
        //            go.AddComponent(addon.type);

        //            addon.created = true;
        //            ++counter;
        //        }
        //    }
        //}
    }
}
