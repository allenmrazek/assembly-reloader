using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Containers;
using ReeperCommon.Extensions;
using ReeperCommon.Extensions.Object;
using UnityEngine;

namespace AssemblyReloader.Queries
{
    class AddonAttributeFromTypeQuery
    {
        public Maybe<KSPAddon> GetKspAddonAttribute(Type type)
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
