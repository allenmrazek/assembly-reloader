using System;

namespace AssemblyReloader.Loaders.PMLoader
{
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
