using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using AssemblyReloader.AddonTracking;
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
        private readonly Log _log;
        private readonly QueryProvider _queryProvider;


        public LoaderFactory(
            GameEventProvider eventProvider,
            IDestructionMediator destructionMediator,
            Log log,
            QueryProvider queryProvider)
        {
            if (eventProvider == null) throw new ArgumentNullException("eventProvider");
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (log == null) throw new ArgumentNullException("log");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _eventProvider = eventProvider;
            _destructionMediator = destructionMediator;
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

            var loader = new Loaders.Addon.AddonLoader(typesThatAreAddons, _eventProvider.GetLevelLoadedEvent(), _destructionMediator, 
                _queryProvider, _log);

            return loader;
        }
    }
}
