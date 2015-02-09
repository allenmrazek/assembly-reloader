using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Loaders.Addon;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Providers.ConfigNodeProviders;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public class LoaderFactory : ILoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IAddonsFromAssemblyQuery _addonsFromAssemblyQuery;
        private readonly IPartModulesFromAssemblyQuery _partModulesFromAssemblyQuery;
        private readonly ICurrentStartupSceneProvider _currentStartupScene;
        private readonly IPartConfigProvider _partConfigProvider;
        private readonly IPartModuleInfoFactory _partModuleInfoFactory;


        public LoaderFactory(
            IAddonFactory addonFactory,
            IAddonsFromAssemblyQuery addonsFromAssemblyQuery,
            IPartModulesFromAssemblyQuery partModulesFromAssemblyQuery,
            ICurrentStartupSceneProvider currentStartupScene,
            IPartConfigProvider partConfigProvider,
            IPartModuleInfoFactory partModuleInfoFactory)
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");
            if (partModulesFromAssemblyQuery == null) throw new ArgumentNullException("partModulesFromAssemblyQuery");
            if (currentStartupScene == null) throw new ArgumentNullException("currentStartupScene");
            if (partConfigProvider == null) throw new ArgumentNullException("partConfigProvider");
            if (partModuleInfoFactory == null) throw new ArgumentNullException("partModuleInfoFactory");


            _addonFactory = addonFactory;
            _addonsFromAssemblyQuery = addonsFromAssemblyQuery;
            _partModulesFromAssemblyQuery = partModulesFromAssemblyQuery;
            _currentStartupScene = currentStartupScene;
            _partConfigProvider = partConfigProvider;
            _partModuleInfoFactory = partModuleInfoFactory;
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

            loader.LoadForScene(_currentStartupScene.Get());

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
                _partModuleInfoFactory,
                log.CreateTag("PartModuleLoader"));

            loader.LoadPartModulesIntoPrefabs();

            if (_currentStartupScene.Get() == KSPAddon.Startup.Flight)
                loader.LoadPartModulesIntoFlight();

            return loader;
        }
    }
}
