﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries.AssemblyQueries;
using UnityEngine;

namespace AssemblyReloader.Destruction
{
    public class AddonDestroyer : IAddonDestroyer
    {
        private readonly IObjectDestructionController _destructionController;
        private readonly ILoadedComponentProvider _componentProvider;
        private readonly ITypesFromAssemblyQuery _addonsFromAssemblyQuery;

        public AddonDestroyer(
            IObjectDestructionController destructionController,
            ILoadedComponentProvider componentProvider,
            ITypesFromAssemblyQuery addonsFromAssemblyQuery)
        {
            if (destructionController == null) throw new ArgumentNullException("destructionController");
            if (componentProvider == null) throw new ArgumentNullException("componentProvider");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");

            _destructionController = destructionController;
            _componentProvider = componentProvider;
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

            foreach (var item in _componentProvider.GetLoaded(type).Cast<MonoBehaviour>())
                _destructionController.Destroy(item);
        }
    }
}
