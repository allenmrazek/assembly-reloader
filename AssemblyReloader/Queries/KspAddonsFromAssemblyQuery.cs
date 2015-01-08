using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using UnityEngine;

namespace AssemblyReloader.Queries
{
    class KspAddonsFromAssemblyQuery
    {
        public IEnumerable<Type> Get(Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => GetKSPAddonFromType(t).Any());
        }



        public Maybe<KSPAddon> GetKSPAddonFromType(Type type)
        {
            if (!type.IsClass || !type.IsSubclassOf(typeof(MonoBehaviour)))
                return Maybe<KSPAddon>.None;

            return Maybe<KSPAddon>
                .With((KSPAddon) type.GetCustomAttributes(true)
                    .FirstOrDefault(attr => attr is KSPAddon));
        }
    }
}
