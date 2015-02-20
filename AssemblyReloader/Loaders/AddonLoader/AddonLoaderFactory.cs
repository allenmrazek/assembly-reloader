using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Loaders.PMLoader;
using AssemblyReloader.Loaders.PMLoader.old;
using AssemblyReloader.Messages;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.Events.Implementations;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public class AddonLoaderFactory : IAddonLoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IAddonsFromAssemblyQuery _addonsFromAssemblyQuery;
        private readonly ICurrentStartupSceneProvider _currentStartupSceneQuery;


        public AddonLoaderFactory(
            IAddonFactory addonFactory,
            IAddonsFromAssemblyQuery addonsFromAssemblyQuery,
            ICurrentStartupSceneProvider currentStartupSceneQuery
            )
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (addonsFromAssemblyQuery == null) throw new ArgumentNullException("addonsFromAssemblyQuery");
            if (currentStartupSceneQuery == null) throw new ArgumentNullException("currentStartupSceneQuery");

            _addonFactory = addonFactory;
            _addonsFromAssemblyQuery = addonsFromAssemblyQuery;
            _currentStartupSceneQuery = currentStartupSceneQuery;
        }





        public IAddonLoader Create(Assembly assembly, ILog log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (log == null) throw new ArgumentNullException("log");

            var typeInfo =
                _addonsFromAssemblyQuery.Get(assembly)
                    .Select(ty => _addonFactory.CreateInfoForAddonType(ty));

            var loader = new AddonLoader(
                _addonFactory,
                typeInfo,
                log);

            loader.LoadForScene(_currentStartupSceneQuery.Get());

            return loader;
        }
    }
}
