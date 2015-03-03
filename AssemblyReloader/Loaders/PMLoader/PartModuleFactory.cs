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
using AssemblyReloader.Queries.CecilQueries;
using AssemblyReloader.Repositories;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.Loaders.PMLoader
{
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly ILoadedPartModuleHandleFactory _handleFactory;
        private readonly IProxyPartModuleTypeProvider _proxyTypeProvider;

        public PartModuleFactory(
            ILoadedPartModuleHandleFactory handleFactory,
            IProxyPartModuleTypeProvider proxyTypeProvider)
        {
            if (handleFactory == null) throw new ArgumentNullException("handleFactory");
            if (proxyTypeProvider == null) throw new ArgumentNullException("proxyTypeProvider");

            _handleFactory = handleFactory;
            _proxyTypeProvider = proxyTypeProvider;
        }


        public ILoadedPartModuleHandle Create(PartModuleDescriptor descriptor)
        {
            return Create(descriptor, descriptor.Config);
        }


        public ILoadedPartModuleHandle Create(PartModuleDescriptor descriptor, ConfigNode withConfig)
        {
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            // create the real thing
            var target = CreatePartModule(descriptor.Type, descriptor.Prefab, withConfig);

            // create proxy which will act as a stand-in for the real one
            var proxy = CreateProxy(_proxyTypeProvider.Get(descriptor.Type), target, descriptor.Prefab, withConfig);

            return _handleFactory.Create(
        }


        private PartModule CreateProxy(Type proxyType, PartModule target, IPart part, ConfigNode config)
        {
            var proxy = CreatePartModule(proxyType, part, config);
        }


        private PartModule CreatePartModule(Type type, IPart part, ConfigNode config)
        {
            var partModule = part.GameObject.AddComponent(type) as PartModule;

            // Awake does some necessary setup
            typeof(PartModule).GetMethod(
                "Awake", 
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy)
                    .Invoke(partModule, null);

            part.Modules.Add(partModule);

            if (partModule == null)
                throw new InvalidOperationException("Failed to add " + type.FullName + " to " + part.PartName);

            partModule.Load(config);

            return partModule;
        }
    }
}
