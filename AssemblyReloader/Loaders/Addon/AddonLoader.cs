﻿using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Loaders.Addon.Factories;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.Implementations;
using AssemblyReloader.TypeTracking;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.Addon
{
    class AddonLoader : IAddonLoader
    {
        // Mainly required so we can flag addons when they've
        // been created in the case of runOnce = true

        private readonly IAddonFactory _addonFactory;
        private readonly IEnumerable<AddonInfo> _addons;
        private readonly ILog _log;
        private readonly List<IDisposable> _created;


        public AddonLoader(
            IAddonFactory addonFactory,
            IEnumerable<AddonInfo> addons,
            ILog log)
        {
            if (addons == null) throw new ArgumentNullException("addons");
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (log == null) throw new ArgumentNullException("log");


            _addonFactory = addonFactory;
            _addons = addons;

            _log = log;
            _created = new List<IDisposable>();
        }



        ~AddonLoader()
        {
            Dispose();
        }



        public void Dispose()
        {
            _created.ForEach(d => d.Dispose());
            GC.SuppressFinalize(this);
        }





        private IEnumerable<AddonInfo> GetAddonsForStartup(KSPAddon.Startup startup)
        {
            return _addons
                .Where(info => info.Scene == startup)
                .Where(info => !info.RunOnce || (info.RunOnce && !info.created));
        }



        public void LoadAddonsForScene(KSPAddon.Startup scene)
        {
            _log.Debug("Loading addons for " + scene);

            var addonsThatMatchScene = GetAddonsForStartup(scene);

            var thatMatchScene = addonsThatMatchScene as AddonInfo[] ?? addonsThatMatchScene.ToArray();

            _log.Verbose(thatMatchScene.Count() + " addons eligible for instantiation");

            foreach (var addonType in thatMatchScene)
                _created.Add(_addonFactory.Create(addonType));
           
        }
    }
}
