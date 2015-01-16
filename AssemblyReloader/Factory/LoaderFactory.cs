﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using AssemblyReloader.Factory.Implementations;
using AssemblyReloader.Loaders;
using AssemblyReloader.Mediators;
using AssemblyReloader.Messages;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    class LoaderFactory
    {
        private readonly GameEventProvider _eventProvider;
        private readonly IDestructionMediator _destructionMediator;
        private readonly IMonoBehaviourFactory _addonFactory;
        private readonly ILog _log;
        private readonly QueryProvider _queryProvider;


        public LoaderFactory(
            GameEventProvider eventProvider,
            IDestructionMediator destructionMediator,
            IMonoBehaviourFactory addonFactory,
            ILog log,
            QueryProvider queryProvider)
        {
            if (eventProvider == null) throw new ArgumentNullException("eventProvider");
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _eventProvider = eventProvider;
            _destructionMediator = destructionMediator;
            _addonFactory = addonFactory;
            _log = log;
            _queryProvider = queryProvider;
        }



        public List<ILoader> CreateLoaders(Assembly assembly, QueryProvider queryProvider)
        {
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _log.Verbose("Creating loaders for assembly '{0}'", assembly.FullName);

            var loaders = new List<ILoader>
            {
                CreateAddonLoader(assembly)
            };


            loaders.ForEach(loader => loader.Initialize());


            return loaders;
        }


        private ILoader CreateAddonLoader(Assembly assembly)
        {
            var typesThatAreAddons = _queryProvider.GetAddonsFromAssemblyQuery(assembly).Get();

            var loader = new Loaders.Addon.AddonLoader(
                typesThatAreAddons, 
                _eventProvider.GetLevelLoadedEvent(), 
                _destructionMediator,
                _addonFactory,
                _queryProvider.GetStartupSceneFromGameSceneQuery(),
                _queryProvider.GetAddonAttributeQuery(), 
                _log.CreateTag("AddonLoader"));

            return loader;
        }
    }
}
