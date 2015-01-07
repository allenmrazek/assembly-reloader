using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.AddonTracking;
using ReeperCommon.Containers;
using UnityEngine;

namespace AssemblyReloader.Providers
{
    class KspAddonProvider
    {
        private readonly Assembly _assembly;

        public KspAddonProvider(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            _assembly = assembly;
        }


        public IEnumerable<Type> Get()
        {
            return _assembly
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
