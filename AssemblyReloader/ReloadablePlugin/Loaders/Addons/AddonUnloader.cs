using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Properties;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class AddonUnloader : IAddonUnloader
    {
        private readonly IGetTypesFromAssembly<AddonType> _getAddonTypesFromAssembly;
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly IGetLoadedUnityComponents _unityComponents;

        public AddonUnloader(
            [NotNull] IGetTypesFromAssembly<AddonType> getAddonTypesFromAssembly,
            [NotNull] IUnityObjectDestroyer objectDestroyer,
            [NotNull] IGetLoadedUnityComponents unityComponents)
        {
            if (getAddonTypesFromAssembly == null) throw new ArgumentNullException("getAddonTypesFromAssembly");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (unityComponents == null) throw new ArgumentNullException("unityComponents");

            _getAddonTypesFromAssembly = getAddonTypesFromAssembly;
            _objectDestroyer = objectDestroyer;
            _unityComponents = unityComponents;
        }


        public void Unload(Assembly assembly)
        {
            foreach (var addonType in _getAddonTypesFromAssembly.Get(assembly))
                DestroyAllInstancesOf(addonType.Type);
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
