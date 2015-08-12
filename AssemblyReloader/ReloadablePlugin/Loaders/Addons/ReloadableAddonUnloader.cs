extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class ReloadableAddonUnloader : IReloadableAddonUnloader
    {
        private readonly IGetAddonTypesForScene _addonTypeQuery;
        private readonly IGetMonoBehavioursInScene _monoBehaviourQuery;
        private readonly IMonoBehaviourDestroyer _addonDestroyer;
        private readonly ILog _log;

        public ReloadableAddonUnloader(
            IGetAddonTypesForScene addonTypeQuery, 
            IGetMonoBehavioursInScene monoBehaviourQuery,
            IMonoBehaviourDestroyer addonDestroyer,
            [Name(LogKey.AddonUnloader)] ILog log)
        {
            if (addonTypeQuery == null) throw new ArgumentNullException("addonTypeQuery");
            if (monoBehaviourQuery == null) throw new ArgumentNullException("monoBehaviourQuery");
            if (addonDestroyer == null) throw new ArgumentNullException("addonDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _addonTypeQuery = addonTypeQuery;
            _monoBehaviourQuery = monoBehaviourQuery;
            _addonDestroyer = addonDestroyer;
            _log = log;
        }


        public int DestroyAddons(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            int counter = 0;

            foreach (var activeInstances in Enum.GetValues(typeof(KSP::KSPAddon.Startup))
                .Cast<KSP::KSPAddon.Startup>()
                .SelectMany(scene => _addonTypeQuery.Get(scene, handle))
                .Select(kvp => kvp.Type)
                .OrderBy(kvp => kvp.FullName)
                .Distinct()
                .Select(addonType => _monoBehaviourQuery.Get(addonType).ToArray()))
            {
                counter += activeInstances.Length;

// ReSharper disable once ForCanBeConvertedToForeach
                for (int idx = 0; idx < activeInstances.Length; ++idx)
                {
                    _log.Verbose("Destroying addon instance: " + activeInstances[idx].name + ", " +
                                 activeInstances[idx].GetType().FullName);
                    _addonDestroyer.DestroyMonoBehaviour(activeInstances[idx]);
                }
            }

            return counter;
        }
    }
}
