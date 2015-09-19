extern alias KSP;
using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    /// <summary>
    /// Each PartModuleDescriptor wraps relevant data about a particular PartModule instance that
    /// should exist on a particular Part prefab
    /// </summary>
    public class PartModuleDescriptor
    {
        public IPart Prefab { get; private set; }
        public KSP::ConfigNode Config { get; private set; }
        public Type Type { get; private set; }
        public TypeIdentifier Identifier { get; private set; }

        public PartModuleDescriptor(IPart prefab, KSP::ConfigNode config, Type type, TypeIdentifier typeIdentifier)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");
            if (config == null) throw new ArgumentNullException("config");
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsSubclassOf(typeof(KSP::PartModule)))
                throw new TypeMustDeriveFromPartModuleException(type);

            Prefab = prefab;
            Config = config;
            Type = type;
            Identifier = typeIdentifier;
        }


        public override string ToString()
        {
            return "PartModuleDescriptor: " + Type.FullName + ", " + Prefab.PartInfo.Name;
        }
    }
}
