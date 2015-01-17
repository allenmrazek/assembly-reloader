using System;
using System.Collections.Generic;
using System.Reflection;
using AssemblyReloader.Events;
using AssemblyReloader.Loaders;
using AssemblyReloader.Mediators;
using AssemblyReloader.Providers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Factory
{
    using AddonLoader = Loaders.Addon.AddonLoader;

    class LoaderFactory
    {
        private readonly IDestructionMediator _destructionMediator;
        private readonly IMonoBehaviourFactory _addonFactory;
        private readonly IGameEventSubscriber<GameScenes> _levelLoadedEvent;
        private readonly ILog _log;
        private readonly QueryProvider _queryProvider;


        public LoaderFactory(
            IDestructionMediator destructionMediator,
            IMonoBehaviourFactory addonFactory,
            IGameEventSubscriber<GameScenes> levelLoadedEvent,
            ILog log,
            QueryProvider queryProvider)
        {
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (levelLoadedEvent == null) throw new ArgumentNullException("levelLoadedEvent");
            if (log == null) throw new ArgumentNullException("log");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _destructionMediator = destructionMediator;
            _addonFactory = addonFactory;
            _levelLoadedEvent = levelLoadedEvent;
            _log = log;
            _queryProvider = queryProvider;
        }



        //public List<ILoader> CreateLoaders(Assembly assembly, QueryProvider queryProvider)
        //{
        //    if (queryProvider == null) throw new ArgumentNullException("queryProvider");

        //    _log.Verbose("Creating loaders for assembly '{0}'", assembly.FullName);

        //    var loaders = new List<ILoader>();

        //    var addonLoader = CreateAddonLoader(assembly);
        //    loaders.Add(addonLoader);



        //    loaders.ForEach(loader => loader.Initialize());


        //    addonLoader.LoadAddonsForScene(queryProvider.GetCurrentGameSceneProvider().Get());


        //    return loaders;
        //}


        public IAddonLoader GetAddonLoader(IEnumerable<Type> addonTypesInAssembly)
        {
            var loader = new Loaders.Addon.AddonLoader(
                addonTypesInAssembly, 
                _destructionMediator,
                _addonFactory,
                _queryProvider.GetStartupSceneFromGameSceneQuery(),
                _queryProvider.GetAddonAttributeQuery(), 
                _log.CreateTag("AddonLoader"));

            return loader;
        }
    }
}
