using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Loaders.Addon.Factories;
using AssemblyReloader.Providers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.Factories.Implementations
{
    class LoaderFactory : ILoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IAddonInfoFactory _infoFactory;
        private readonly ILog _log;
        private readonly QueryProvider _queryProvider;


        public LoaderFactory(
            IAddonFactory addonFactory,
            IAddonInfoFactory infoFactory,
            ILog log,
            QueryProvider queryProvider)
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");

            _addonFactory = addonFactory;
            _infoFactory = infoFactory;
            _log = log;
            _queryProvider = queryProvider;
        }





        public IAddonLoader CreateAddonLoader(Assembly assembly)
        {
            var typeInfo =
                _queryProvider.GetAddonsFromAssemblyQuery(assembly).Get().Select(ty => _infoFactory.Create(ty));

            var loader = new Loaders.Addon.AddonLoader(
                _addonFactory,
                typeInfo,
                _queryProvider.GetStartupSceneFromGameSceneQuery(),
                _log.CreateTag("AddonLoader"));

            return loader;
        }
    }
}
