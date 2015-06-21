﻿using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using UnityEngine;

namespace AssemblyReloader.Queries
{
    [Implements(typeof(IAddonAttributesFromTypeQuery))]
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
