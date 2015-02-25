using System;
using System.Reflection;
using AssemblyReloader.Game;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleFactory : IPartModuleFactory
    {
        private PartModule DoAddModule(Type type, IPart part, ConfigNode config, bool forceAwake = false)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (part == null) throw new ArgumentNullException("part");
            if (!typeof(PartModule).IsAssignableFrom(type))
                throw new ArgumentException(type.FullName + " must be a PartModule");

            var pm = part.GameObject.AddComponent(type) as PartModule;

            if (pm.IsNull())
            {
                throw new InvalidOperationException(string.Format("Failed to add {0} to {1}; AddComponent returned null", type.FullName,
                    part.Name));
            }

            part.Modules.Add(pm);

            if (forceAwake)
                typeof(PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .Invoke(pm, null);

            pm.Load(config);

            return pm;
        }


        public PartModule Create(PartModuleDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            return DoAddModule(descriptor.Type, descriptor.Prefab, descriptor.Config, true);
        }


        public PartModule Create(IPart part, ConfigNode config, Type pmType)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (config == null) throw new ArgumentNullException("config");
            if (pmType == null) throw new ArgumentNullException("pmType");

            return DoAddModule(pmType, part, config, true);
        }
    }
}
