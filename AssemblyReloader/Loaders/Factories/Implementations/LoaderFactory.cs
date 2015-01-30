using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Loaders.Addon.Factories;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.Implementations;
using AssemblyReloader.Queries.Implementations;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.Factories.Implementations
{
    class LoaderFactory : ILoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IAddonInfoFactory _infoFactory;
        private readonly ILog _log;
        private readonly QueryFactory _queryFactory;


        public LoaderFactory(
            IAddonFactory addonFactory,
            IAddonInfoFactory infoFactory,
            ILog log,
            QueryFactory queryFactory)
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");

            _addonFactory = addonFactory;
            _infoFactory = infoFactory;
            _log = log;
            _queryFactory = queryFactory;
        }





        public IAddonLoader CreateAddonLoader(Assembly assembly)
        {
            var typeInfo =
                _queryFactory.GetAddonsFromAssemblyQuery(assembly).Get().Select(ty => _infoFactory.Create(ty));

            var loader = new Loaders.Addon.AddonLoader(
                _addonFactory,
                typeInfo,
                _log.CreateTag("AddonLoader"));

            return loader;
        }
    }
}
