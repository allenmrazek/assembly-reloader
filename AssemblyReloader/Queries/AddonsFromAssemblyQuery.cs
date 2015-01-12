using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Queries
{
    class AddonsFromAssemblyQuery
    {
        public IEnumerable<Type> Get(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return assembly
                .GetTypes()
                .Where(t => GetKSPAddonFromType(t).Any());
        }


        public Maybe<KSPAddon> GetKSPAddonFromType(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (!type.IsClass || !type.IsSubclassOf(typeof(MonoBehaviour)))
                return Maybe<KSPAddon>.None;

            var addon = type.GetCustomAttributes(true)
                            .FirstOrDefault(attr => attr is KSPAddon) as KSPAddon;

            return addon.IsNull() ? Maybe<KSPAddon>.None : Maybe<KSPAddon>.With(addon);
        }
    }
}
