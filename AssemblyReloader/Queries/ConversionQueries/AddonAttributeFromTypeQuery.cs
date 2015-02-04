using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Queries.ConversionQueries
{
    public class AddonAttributeFromTypeQuery : IAddonAttributeFromTypeQuery
    {
        public Maybe<KSPAddon> Get(Type type)
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
