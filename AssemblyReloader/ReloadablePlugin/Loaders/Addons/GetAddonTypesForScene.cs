extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using ReeperAssemblyLibrary;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetAddonTypesForScene : IGetAddonTypesForScene
    {
        private readonly IGetAttributesOfType<ReloadableAddonAttribute> _getAttributes;

        public GetAddonTypesForScene(IGetAttributesOfType<ReloadableAddonAttribute> getAttributes)
        {
            if (getAttributes == null) throw new ArgumentNullException("getAttributes");
            _getAttributes = getAttributes;
        }


        public IEnumerable<ReloadableAddonType> Get(KSP::KSPAddon.Startup scene, ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            return handle.LoadedAssembly.assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof (MonoBehaviour)))
                .Where(
                    t =>
                        _getAttributes.Get(t)
                            .Any(attr => attr.startup == scene || attr.startup == KSP::KSPAddon.Startup.EveryScene))
                .Select(t => new ReloadableAddonType(t, _getAttributes.Get(t).Single()));
        }
    }
}
