using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class ReloadableAddonType
    {
        public Type Type { get; private set; }
        public ReloadableAddonAttribute Attribute { get; private set; }

        public ReloadableAddonType(Type type, ReloadableAddonAttribute attribute)
        {
            Type = type;
            Attribute = attribute;
        }
    }
}
