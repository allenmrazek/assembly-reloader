using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly IAssemblyProvider<ITypeIdentifier> _proxyProvider;
        private readonly ITypeIdentifierQuery _identifierQuery;
        private readonly ITypesFromAssemblyQuery _partModulesFromAssemblyQuery;
        private const string ProxyTargetField = "Target";

        public PartModuleFactory(
            IAssemblyProvider<ITypeIdentifier> proxyProvider,
            ITypeIdentifierQuery identifierQuery,
            ITypesFromAssemblyQuery partModulesFromAssemblyQuery)
        {
            if (proxyProvider == null) throw new ArgumentNullException("proxyProvider");
            if (identifierQuery == null) throw new ArgumentNullException("identifierQuery");
            if (partModulesFromAssemblyQuery == null) throw new ArgumentNullException("partModulesFromAssemblyQuery");
            _proxyProvider = proxyProvider;
            _identifierQuery = identifierQuery;
            _partModulesFromAssemblyQuery = partModulesFromAssemblyQuery;
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

            var proxyPartModule = CreateProxyFor(descriptor);
           
            var targetPartModule = DoAddModule(descriptor.Type, descriptor.Prefab, descriptor.Config, true);

            SetTarget(proxyPartModule, descriptor.Type);

            return targetPartModule;
        }


        private PartModule CreateProxyFor(PartModuleDescriptor descriptor)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            var proxyAssembly = _proxyProvider.Get(_identifierQuery.Get(descriptor.Type));
            if (!proxyAssembly.Any())
                throw new InvalidOperationException("Failed to create proxy PartModule assembly for " + descriptor.Type.FullName);

            var pms = _partModulesFromAssemblyQuery.Get(proxyAssembly.Single()).ToList();

            if (pms.Count != 1)
                throw new InvalidOperationException("Failed to find proxy PartModule from " +
                                                    proxyAssembly.Single().FullName + "; found " + pms.Count +
                                                    " items (1 expected)");

            if (!IsPartModule(pms.Single()))
                throw new InvalidOperationException("Proxy assembly did not provide a PartModule");

            var proxyPartModule = DoAddModule(pms.Single(), descriptor.Prefab, descriptor.Config, true);

            if (proxyPartModule.IsNull())
                throw new Exception("Failed to create proxy PartModule on prefab for " + descriptor.Type.FullName);

            return proxyPartModule;
        }


        private bool IsPartModule(Type type)
        {
            return typeof (PartModule).IsAssignableFrom(type);
        }


        private void SetTarget(PartModule proxy, Type target)
        {
            if (proxy == null) throw new ArgumentNullException("proxy");
            if (target == null) throw new ArgumentNullException("target");

            var field = proxy.GetType().GetField(ProxyTargetField, BindingFlags.Instance | BindingFlags.Public);
            if (field == null)
                throw new MissingFieldException(proxy.GetType().FullName, ProxyTargetField);

            field.SetValue(proxy, target);
        }

    }
}
