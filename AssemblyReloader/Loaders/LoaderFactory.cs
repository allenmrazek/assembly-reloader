using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Addon;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    class LoaderFactory : ILoaderFactory
    {
        private readonly IAddonFactory _addonFactory;
        private readonly IQueryFactory _queryFactory;


        public LoaderFactory(
            IAddonFactory addonFactory,
            IQueryFactory queryFactory)
        {
            if (addonFactory == null) throw new ArgumentNullException("addonFactory");
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");


            _addonFactory = addonFactory;
            _queryFactory = queryFactory;
        }





        public IAddonLoader CreateAddonLoader(Assembly assembly, ILog log)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (log == null) throw new ArgumentNullException("log");

            var typeInfo =
                _queryFactory.GetAddonsFromAssemblyQuery(assembly).Get()
                    .Select(ty => _addonFactory.CreateInfoForAddonType(ty));

            var loader = new AddonLoader(
                _addonFactory,
                typeInfo,
                log);

            return loader;
        }
    }
}
