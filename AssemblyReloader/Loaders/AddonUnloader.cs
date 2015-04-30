using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using UnityEngine;

namespace AssemblyReloader.Loaders
{
    public class AddonUnloader : IAddonUnloader
    {
        private readonly ITypesFromAssemblyQuery _addonTypesFromAssemblyQuery;
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly ILoadedComponentQuery _componentQuery;

        public AddonUnloader(
            [NotNull] ITypesFromAssemblyQuery addonTypesFromAssemblyQuery,
            [NotNull] IUnityObjectDestroyer objectDestroyer,
            [NotNull] ILoadedComponentQuery componentQuery)
        {
            if (addonTypesFromAssemblyQuery == null) throw new ArgumentNullException("addonTypesFromAssemblyQuery");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (componentQuery == null) throw new ArgumentNullException("componentQuery");

            _addonTypesFromAssemblyQuery = addonTypesFromAssemblyQuery;
            _objectDestroyer = objectDestroyer;
            _componentQuery = componentQuery;
        }


        public void Unload(Assembly assembly)
        {
            foreach (var addonType in _addonTypesFromAssemblyQuery.Get(assembly))
                DestroyAllInstancesOf(addonType);
        }


        private IEnumerable<UnityEngine.Object> GetLoadedInstancesOfType(Type type)
        {
            return _componentQuery.GetLoaded(type).Where(t => t.GetType().IsSubclassOf(typeof (MonoBehaviour)));
        }


        private void DestroyAllInstancesOf(Type type)
        {
            foreach (var instance in GetLoadedInstancesOfType(type))
                _objectDestroyer.Destroy(instance);
        }
    }
}
