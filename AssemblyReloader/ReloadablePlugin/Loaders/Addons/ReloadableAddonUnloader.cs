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
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IGetAddonTypesForScene _addonTypeQuery;
        private readonly IGetMonoBehavioursInScene _monoBehaviourQuery;
        private readonly IUnityObjectDestroyer _addonDestroyer;
        private readonly ILog _log;

        public ReloadableAddonUnloader(
            ILoadedAssemblyHandle handle,
            IGetAddonTypesForScene addonTypeQuery, 
            IGetMonoBehavioursInScene monoBehaviourQuery,
            IUnityObjectDestroyer addonDestroyer,
            [Name(LogKeys.AddonUnloader)] ILog log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (addonTypeQuery == null) throw new ArgumentNullException("addonTypeQuery");
            if (monoBehaviourQuery == null) throw new ArgumentNullException("monoBehaviourQuery");
            if (addonDestroyer == null) throw new ArgumentNullException("addonDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _addonTypeQuery = addonTypeQuery;
            _monoBehaviourQuery = monoBehaviourQuery;
            _addonDestroyer = addonDestroyer;
            _log = log;
        }


        public int DestroyAddons()
        {
            int counter = 0;

            foreach (var activeInstances in Enum.GetValues(typeof (KSPAddon.Startup))
                .Cast<KSPAddon.Startup>()
                .SelectMany(scene => _addonTypeQuery.Get(scene, _handle))
                .Select(kvp => kvp.Key)
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
                    _addonDestroyer.Destroy(activeInstances[idx]);
                }
            }

            return counter;
        }
    }
}
