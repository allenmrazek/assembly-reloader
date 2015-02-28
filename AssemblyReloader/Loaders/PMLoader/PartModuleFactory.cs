using System;
using System.Reflection;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly IAssemblyProvider<ITypeIdentifier> _proxyProvider;

        public PartModuleFactory(IAssemblyProvider<ITypeIdentifier> proxyProvider)
        {
            if (proxyProvider == null) throw new ArgumentNullException("proxyProvider");
            _proxyProvider = proxyProvider;
        }


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
    }
}
