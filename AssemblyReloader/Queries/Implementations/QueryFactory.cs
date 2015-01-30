using System;
using System.Reflection;
using AssemblyReloader.Providers;

namespace AssemblyReloader.Queries.Implementations
{
    public class QueryFactory : IQueryFactory
    {
        private readonly AddonAttributeFromTypeQuery _attributeQuery;
        private readonly StartupSceneFromGameSceneQuery _startupSceneConverter;
        private readonly CurrentGameSceneQuery _currentGameSceneQuery;



        public QueryFactory()
        {
            _attributeQuery = new AddonAttributeFromTypeQuery();
            _startupSceneConverter = new StartupSceneFromGameSceneQuery();
            _currentGameSceneQuery = new CurrentGameSceneQuery();
        }



        public IAddonsFromAssemblyQuery GetAddonsFromAssemblyQuery(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return new AddonsFromAssemblyQuery(assembly, _attributeQuery);
        }



        public IAddonAttributeFromTypeQuery GetAddonAttributeQuery()
        {
            return new AddonAttributeFromTypeQuery();
        }



        public IStartupSceneFromGameSceneQuery GetStartupSceneFromGameSceneQuery()
        {
            return new StartupSceneFromGameSceneQuery();
        }



        public ICurrentGameSceneQuery GetCurrentGameSceneProvider()
        {
            return new CurrentGameSceneQuery();
        }
    }
}
