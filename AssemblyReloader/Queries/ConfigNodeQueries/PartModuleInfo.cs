using System;
using UnityEngine;

namespace AssemblyReloader.Queries.ConfigNodeQueries
{
    public class PartModuleInfo
    {
        public GameObject Prefab { get; private set; }
        public ConfigNode Config { get; private set; }
        public Type PmType { get; private set; }

        public PartModuleInfo(GameObject prefab, ConfigNode config, Type pmType)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");
            if (config == null) throw new ArgumentNullException("config");
            if (pmType == null) throw new ArgumentNullException("pmType");

            Prefab = prefab;
            Config = config;
            PmType = pmType;
        }
    }
}
