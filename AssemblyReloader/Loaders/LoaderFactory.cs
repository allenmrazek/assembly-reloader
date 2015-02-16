using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.Loaders.Addon;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Repositories;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public class LoaderFactory : ILoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IAddonsFromAssemblyQuery _addonsFromAssemblyQuery;
        private readonly IPartModulesFromAssemblyQuery _partModulesFromAssemblyQuery;
        private readonly ICurrentStartupSceneProvider _currentStartupSceneQuery;
        private readonly ICurrentGameSceneProvider _currentGameSceneQuery;
        private readonly IPartModuleInfoFactory _partModuleInfoFactory;
        private readonly IPartModuleFlightConfigRepository _pmConfigRepository;


        public LoaderFactory(
            IAddonFactory addonFactory,
            IPartModuleFactory partModuleFactory,
            IPartModuleInfoFactory partModuleInfoFactory,

            IPartModuleFlightConfigRepository pmConfigRepository,

        
            IAddonsFromAssemblyQuery addonsFromAssemblyQuery,
            IPartModulesFromAssemblyQuery partModulesFromAssemblyQuery,
            ICurrentStartupSceneProvider currentStartupSceneQuery,
            ICurrentGameSceneProvider currentGameSceneQuery
            )
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");
            if (partModulesFromAssemblyQuery == null) throw new ArgumentNullException("partModulesFromAssemblyQuery");
            if (currentStartupSceneQuery == null) throw new ArgumentNullException("currentStartupSceneQuery");
            if (currentGameSceneQuery == null) throw new ArgumentNullException("currentGameSceneQuery");
            if (partModuleInfoFactory == null) throw new ArgumentNullException("partModuleInfoFactory");
            if (pmConfigRepository == null) throw new ArgumentNullException("pmConfigRepository");


            _addonFactory = addonFactory;
            _partModuleFactory = partModuleFactory;
            _addonsFromAssemblyQuery = addonsFromAssemblyQuery;
            _partModulesFromAssemblyQuery = partModulesFromAssemblyQuery;
            _currentStartupSceneQuery = currentStartupSceneQuery;
            _currentGameSceneQuery = currentGameSceneQuery;
            _partModuleInfoFactory = partModuleInfoFactory;
            _pmConfigRepository = pmConfigRepository;
        }





        public IAddonLoader CreateAddonLoader(Assembly assembly, ILog log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (log == null) throw new ArgumentNullException("log");

            var typeInfo =
                _addonsFromAssemblyQuery.Get(assembly)
                    .Select(ty => _addonFactory.CreateInfoForAddonType(ty));

            var loader = new Addon.AddonLoader(
                _addonFactory,
                typeInfo,
                log);

            loader.LoadForScene(_currentStartupSceneQuery.Get());

            return loader;
        }



        public IDisposable CreatePartModuleLoader(Assembly assembly, ILog log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (log == null) throw new ArgumentNullException("log");

            log.Normal("Listing PartModules from " + assembly.FullName);
            _partModulesFromAssemblyQuery.Get(assembly).ToList().ForEach(t => log.Normal("PartModule: " + t.FullName));

            var loader = new PartModuleLoader(
                _partModulesFromAssemblyQuery.Get(assembly), 
                _pmConfigRepository,
                _partModuleFactory,
                _partModuleInfoFactory,
                log.CreateTag("PartModuleLoader"));

            loader.LoadPartModulesIntoPrefabs();

            if (_currentStartupSceneQuery.Get() == KSPAddon.Startup.Flight)
                loader.LoadPartModulesIntoFlight();

            return loader;
        }
    }
}
