extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using ReeperAssemblyLibrary;
using UnityEngine;
using KSPAddon = KSP::KSPAddon;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetAddonTypesForScene : IGetAddonTypesForScene
    {
        private readonly IGetAttributesOfType<ReloadableAddonAttribute> _getAttributes;
        private readonly IAddonSettings _addonSettings;

        public GetAddonTypesForScene(IGetAttributesOfType<ReloadableAddonAttribute> getAttributes,
            IAddonSettings addonSettings)
        {
            if (getAttributes == null) throw new ArgumentNullException("getAttributes");
            if (addonSettings == null) throw new ArgumentNullException("addonSettings");
            _getAttributes = getAttributes;
            _addonSettings = addonSettings;
        }


        public IEnumerable<ReloadableAddonType> Get(KSP::KSPAddon.Startup scene, ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            return handle.LoadedAssembly.assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (MonoBehaviour)))
                .Where(
                    t =>
                        _getAttributes.Get(t)
                            .Any(attr => KSPAddonStartupMatchesScene(attr.startup, scene)))
                .Select(t => new ReloadableAddonType(t, _getAttributes.Get(t).Single()));
        }


// ReSharper disable once InconsistentNaming
        private bool KSPAddonStartupMatchesScene(KSPAddon.Startup startup, KSPAddon.Startup scene)
        {
            if (startup == KSPAddon.Startup.Instantly && _addonSettings.InstantAppliesToEveryScene)
                return true;

            if (startup == KSPAddon.Startup.EveryScene)
                return true;

            return startup == scene;
        }
    }
}
