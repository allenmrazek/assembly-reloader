using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using UnityEngine;

namespace AssemblyReloader.Destruction
{
    public class AddonDestroyer : IAddonDestroyer
    {
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly ILoadedComponentQuery _componentQuery;
        private readonly ITypesFromAssemblyQuery _addonsFromAssemblyQuery;

        public AddonDestroyer(
            IUnityObjectDestroyer objectDestroyer,
            ILoadedComponentQuery componentQuery,
            ITypesFromAssemblyQuery addonsFromAssemblyQuery)
        {
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (componentQuery == null) throw new ArgumentNullException("componentQuery");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");


            _objectDestroyer = objectDestroyer;
            _componentQuery = componentQuery;
            _addonsFromAssemblyQuery = addonsFromAssemblyQuery;
        }


        public void DestroyAddonsFrom(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _addonsFromAssemblyQuery.Get(assembly).ToList().ForEach(DestroyAddonsOfType);
        }


        private void DestroyAddonsOfType(Type type)
        {
            if (!type.IsSubclassOf(typeof (UnityEngine.MonoBehaviour)))
                throw new ArgumentException(type.FullName +
                                            " is not derived from UnityEngine.MonoBehaviour! Bad KSPAddon attribute?");

            foreach (var item in _componentQuery.GetLoaded(type).Cast<MonoBehaviour>())
                _objectDestroyer.Destroy(item);
        }
    }
}
