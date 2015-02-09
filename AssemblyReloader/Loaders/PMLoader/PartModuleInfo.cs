using System;

namespace AssemblyReloader.Loaders.PMLoader
{
    /// <summary>
    /// Each PartModuleInfo wraps relevant data about a particular PartModule instance that
    /// should exist on a particular Part prefab
    /// </summary>
    public class PartModuleInfo
    {
        public Part Prefab { get; private set; }
        public ConfigNode Config { get; private set; }
        public Type PmType { get; private set; }

        public PartModuleInfo(Part prefab, ConfigNode config, Type pmType)
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
