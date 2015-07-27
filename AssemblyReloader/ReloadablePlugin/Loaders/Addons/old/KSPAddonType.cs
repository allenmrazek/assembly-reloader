//using System;
//using System.Linq;
//using UnityEngine;

//namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
//{
//// ReSharper disable once InconsistentNaming
//    public class KSPAddonType
//    {
//        public Type Type { get; private set; }
//        public KSPAddon Attribute { get; private set; }

//        public KSPAddonType(Type addonType)
//        {
//            if (addonType == null) throw new ArgumentNullException("addonType");

//            if (!addonType.IsSubclassOf(typeof (MonoBehaviour)))
//                throw new ArgumentException("addonType must inherit MonoBehaviour");

//            var attr = addonType.GetCustomAttributes(typeof (KSPAddon), false);
//            if (!attr.Any())
//                throw new ArgumentException("addonType must have KSPAddon attribute");

//            Type = addonType;
//            Attribute = attr.First() as KSPAddon;
//        }
//    }
//}
