using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Queries.ConversionQueries
{
    public class AddonAttributesFromTypeQuery : IAddonAttributesFromTypeQuery
    {
        public IEnumerable<KSPAddon> Get(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (!type.IsClass || !type.IsSubclassOf(typeof(MonoBehaviour)))
                return Maybe<KSPAddon>.None;

            return type.GetCustomAttributes(true).OfType<KSPAddon>();
        }
    }
}
