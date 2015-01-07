using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Providers;

namespace AssemblyReloader.Loaders
{
    class KspAddonLoader : ILoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true
        private readonly List<AddonInfo> _addons;

        private readonly KspAddonProvider _provider;



        public KspAddonLoader(KspAddonProvider provider)
        {
            if (provider == null) throw new ArgumentNullException("provider");

            _provider = provider;
            _addons = new List<AddonInfo>();
        }




        public void DoLevelLoad(KSPAddon.Startup scene)
        {
            // identify KSPAddons which match our scene
            var addons =
                _provider.Get()
                    .Select(t => _provider.GetKSPAddonFromType(t))
                    .Where(addon => addon.First().startup == scene);


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
