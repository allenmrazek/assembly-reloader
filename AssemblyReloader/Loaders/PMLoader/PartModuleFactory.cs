using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Repositories;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleFactory : IPartModuleFactory
    {


        public IDisposable Create(Type type, GameObject gameObject, ConfigNode config)
        {
            // note: don't forget to create proxy partmodule too!
            throw new NotImplementedException();
        }


        //private readonly IPartModuleFlightConfigRepository _configRepository;
        //private readonly IProxyPartModuleTypeProvider _proxyTypeProvider;
        //private readonly ITypeIdentifierQuery _identifierQuery;
        //private readonly ITypesFromAssemblyQuery _partModulesFromAssemblyQuery;
        //private readonly ICurrentSceneIsFlightQuery _inflightQuery;
        //private readonly IPartModuleUnloader _partModuleUnloader;


        //public PartModuleFactory(
        //    IPartModuleFlightConfigRepository configRepository,
        //    IProxyPartModuleTypeProvider proxyTypeProvider,
        //    ITypeIdentifierQuery identifierQuery,
        //    ITypesFromAssemblyQuery partModulesFromAssemblyQuery,
        //    ICurrentSceneIsFlightQuery inflightQuery,
        //    IPartModuleUnloader partModuleUnloader)
        //{
        //    if (configRepository == null) throw new ArgumentNullException("configRepository");
        //    if (proxyTypeProvider == null) throw new ArgumentNullException("proxyTypeProvider");
        //    if (identifierQuery == null) throw new ArgumentNullException("identifierQuery");
        //    if (partModulesFromAssemblyQuery == null) throw new ArgumentNullException("partModulesFromAssemblyQuery");
        //    if (inflightQuery == null) throw new ArgumentNullException("inflightQuery");
        //    if (partModuleUnloader == null) throw new ArgumentNullException("partModuleUnloader");

        //    _configRepository = configRepository;
        //    _proxyTypeProvider = proxyTypeProvider;
        //    _identifierQuery = identifierQuery;
        //    _partModulesFromAssemblyQuery = partModulesFromAssemblyQuery;
        //    _inflightQuery = inflightQuery;
        //    _partModuleUnloader = partModuleUnloader;
        //}


        //public IEnumerable<IDisposable> Create(IEnumerable<PartModuleDescriptor> descriptors)
        //{
        //    return descriptors.SelectMany(Create);
        //}


        //private IEnumerable<IDisposable> Create(PartModuleDescriptor descriptor)
        //{
        //    // prefab
        //    var prefabTarget = AddPartModule(descriptor.Type, descriptor.Prefab, descriptor.Config);
        //    var prefabProxy = AddPartModule(_proxyTypeProvider.Get(descriptor), descriptor.Prefab, new ConfigNode());

        //    return new LoadedPartModule(descriptor, _partModuleUnloader);
        //}



   


        //private PartModule AddPartModule(Type type, IPart part, ConfigNode config)
        //{
            
        //}

        //private readonly IAssemblyProvider<ITypeIdentifier> _proxyProvider;
        //private readonly ITypeIdentifierQuery _identifierQuery;
        //private readonly ITypesFromAssemblyQuery _partModulesFromAssemblyQuery;
        //private const string ProxyTargetField = "Target";

        //public PartModuleFactory(
        //    IAssemblyProvider<ITypeIdentifier> proxyProvider,
        //    ITypeIdentifierQuery identifierQuery,
        //    ITypesFromAssemblyQuery partModulesFromAssemblyQuery)
        //{
        //    if (proxyProvider == null) throw new ArgumentNullException("proxyProvider");
        //    if (identifierQuery == null) throw new ArgumentNullException("identifierQuery");
        //    if (partModulesFromAssemblyQuery == null) throw new ArgumentNullException("partModulesFromAssemblyQuery");
        //    _proxyProvider = proxyProvider;
        //    _identifierQuery = identifierQuery;
        //    _partModulesFromAssemblyQuery = partModulesFromAssemblyQuery;
        //}


        //private PartModule DoAddModule(Type type, IPart part, ConfigNode config, bool forceAwake = false)
        //{
        //    if (type == null) throw new ArgumentNullException("type");
        //    if (part == null) throw new ArgumentNullException("part");
        //    if (!typeof(PartModule).IsAssignableFrom(type))
        //        throw new ArgumentException(type.FullName + " must be a PartModule");

        //    var pm = part.GameObject.AddComponent(type) as PartModule;

        //    if (pm.IsNull())
        //    {
        //        throw new InvalidOperationException(string.Format("Failed to add {0} to {1}; AddComponent returned null", type.FullName,
        //            part.Name));
        //    }

        //    part.Modules.Add(pm);

        //    if (forceAwake)
        //        typeof(PartModule).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
        //            .Invoke(pm, null);

        //    pm.LoadPartModules(config);

        //    return pm;
        //}


        //public PartModule Create(PartModuleDescriptor descriptor)
        //{
        //    if (descriptor == null) throw new ArgumentNullException("descriptor");

        //    var proxyPartModule = CreateProxyFor(descriptor);
           
        //    var targetPartModule = DoAddModule(descriptor.Type, descriptor.Prefab, descriptor.Config, true);

        //    SetTarget(proxyPartModule, descriptor.Type);

        //    return targetPartModule;
        //}


        //private PartModule CreateProxyFor(PartModuleDescriptor descriptor)
        //{
        //    if (descriptor == null) throw new ArgumentNullException("descriptor");

        //    var proxyAssembly = _proxyProvider.Get(_identifierQuery.Get(descriptor.Type));
        //    if (!proxyAssembly.Any())
        //        throw new InvalidOperationException("Failed to create proxy PartModule assembly for " + descriptor.Type.FullName);

        //    var pms = _partModulesFromAssemblyQuery.Get(proxyAssembly.Single()).ToList();

        //    if (pms.Count != 1)
        //        throw new InvalidOperationException("Failed to find proxy PartModule from " +
        //                                            proxyAssembly.Single().FullName + "; found " + pms.Count +
        //                                            " items (1 expected)");

        //    if (!IsPartModule(pms.Single()))
        //        throw new InvalidOperationException("Proxy assembly did not provide a PartModule");

        //    var proxyPartModule = DoAddModule(pms.Single(), descriptor.Prefab, descriptor.Config, true);

        //    if (proxyPartModule.IsNull())
        //        throw new Exception("Failed to create proxy PartModule on prefab for " + descriptor.Type.FullName);

        //    return proxyPartModule;
        //}


        //private bool IsPartModule(Type type)
        //{
        //    return typeof (PartModule).IsAssignableFrom(type);
        //}


        //private void SetTarget(PartModule proxy, Type target)
        //{
        //    if (proxy == null) throw new ArgumentNullException("proxy");
        //    if (target == null) throw new ArgumentNullException("target");

        //    var field = proxy.GetType().GetField(ProxyTargetField, BindingFlags.Instance | BindingFlags.Public);
        //    if (field == null)
        //        throw new MissingFieldException(proxy.GetType().FullName, ProxyTargetField);

        //    field.SetValue(proxy, target);
        //}


  
    }
}
