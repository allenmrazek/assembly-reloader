using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Loaders.PMLoader.old;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public class AddonLoaderFactory : IAddonLoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IAddonsFromAssemblyQuery _addonsFromAssemblyQuery;
        private readonly IPartModulesFromAssemblyQuery _partModulesFromAssemblyQuery;
        private readonly ICurrentStartupSceneProvider _currentStartupSceneQuery;
        private readonly IPartModuleInfoFactory _partModuleInfoFactory;


        public AddonLoaderFactory(
            IAddonFactory addonFactory,
            IPartModuleFactory partModuleFactory,
            IPartModuleInfoFactory partModuleInfoFactory,
        
            IAddonsFromAssemblyQuery addonsFromAssemblyQuery,
            IPartModulesFromAssemblyQuery partModulesFromAssemblyQuery,
            ICurrentStartupSceneProvider currentStartupSceneQuery
            )
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");
            if (partModulesFromAssemblyQuery == null) throw new ArgumentNullException("partModulesFromAssemblyQuery");
            if (currentStartupSceneQuery == null) throw new ArgumentNullException("currentStartupSceneQuery");
            if (partModuleInfoFactory == null) throw new ArgumentNullException("partModuleInfoFactory");


            _addonFactory = addonFactory;
            _partModuleFactory = partModuleFactory;
            _addonsFromAssemblyQuery = addonsFromAssemblyQuery;
            _partModulesFromAssemblyQuery = partModulesFromAssemblyQuery;
            _currentStartupSceneQuery = currentStartupSceneQuery;
            _partModuleInfoFactory = partModuleInfoFactory;
        }





        public IAddonLoader Create(Assembly assembly, ILog log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (log == null) throw new ArgumentNullException("log");

            var typeInfo =
                _addonsFromAssemblyQuery.Get(assembly)
                    .Select(ty => _addonFactory.CreateInfoForAddonType(ty));

            var loader = new Loaders.AddonLoader.AddonLoader(
                _addonFactory,
                typeInfo,
                log);

            loader.LoadForScene(_currentStartupSceneQuery.Get());

            return loader;
        }



        //public IDisposable CreatePartModuleLoader(Assembly assembly, ILog log)
        //{
        //    if (assembly == null) throw new ArgumentNullException("assembly");
        //    if (log == null) throw new ArgumentNullException("log");

        //    log.Normal("Listing PartModules from " + assembly.FullName);
        //    _partModulesFromAssemblyQuery.Get(assembly).ToList().ForEach(t => log.Normal("PartModule: " + t.FullName));

        //    var loader = new PartModuleLoader(
        //        _partModulesFromAssemblyQuery.Get(assembly), 
        //        _partModuleFactory,
        //        _partModuleInfoFactory,
        //        log.CreateTag("PartModuleLoader"));

        //    loader.LoadPartModulesIntoPrefabs();

        //    //if (_currentStartupSceneQuery.Get() == KSPAddon.Startup.Flight)
        //    //    loader.LoadPartModulesIntoFlight();

        //    return loader;
        //}
    }
}
