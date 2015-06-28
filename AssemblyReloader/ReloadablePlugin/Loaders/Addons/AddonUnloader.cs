using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Game.Queries;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class AddonUnloader : IAddonUnloader
    {
        private readonly IGetTypesFromAssembly _addonGetTypesFromAssembly;
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly IGetLoadedUnityComponents _unityComponents;

        public AddonUnloader(
            [NotNull] IGetTypesFromAssembly addonGetTypesFromAssembly,
            [NotNull] IUnityObjectDestroyer objectDestroyer,
            [NotNull] IGetLoadedUnityComponents unityComponents)
        {
            if (addonGetTypesFromAssembly == null) throw new ArgumentNullException("addonGetTypesFromAssembly");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (unityComponents == null) throw new ArgumentNullException("unityComponents");

            _addonGetTypesFromAssembly = addonGetTypesFromAssembly;
            _objectDestroyer = objectDestroyer;
            _unityComponents = unityComponents;
        }


        public void Unload(Assembly assembly)
        {
            foreach (var addonType in _addonGetTypesFromAssembly.Get(assembly))
                DestroyAllInstancesOf(addonType);
        }


        private IEnumerable<UnityEngine.Object> GetLoadedInstancesOfType(Type type)
        {
            return _unityComponents.GetLoaded(type).Where(t => t.GetType().IsSubclassOf(typeof (MonoBehaviour)));
        }


        private void DestroyAllInstancesOf(Type type)
        {
            foreach (var instance in GetLoadedInstancesOfType(type))
                _objectDestroyer.Destroy(instance);
        }
    }
}
