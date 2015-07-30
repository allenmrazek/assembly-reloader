using System;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    /// <summary>
    /// Each PartModuleDescriptor wraps relevant data about a particular PartModule instance that
    /// should exist on a particular Part prefab
    /// </summary>
    public class PartModuleDescriptor
    {
        public IPart Prefab { get; private set; }
        public ConfigNode Config { get; private set; }
        public Type Type { get; private set; }
        public ITypeIdentifier Identifier { get; private set; }

        public PartModuleDescriptor(IPart prefab, ConfigNode config, Type type, ITypeIdentifier typeIdentifier)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");
            if (config == null) throw new ArgumentNullException("config");
            if (type == null) throw new ArgumentNullException("type");

            Prefab = prefab;
            Config = config;
            Type = type;
            Identifier = typeIdentifier;
        }

    }
}
