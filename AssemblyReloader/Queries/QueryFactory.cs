using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using AssemblyReloader.Queries.ConversionQueries;

namespace AssemblyReloader.Queries
{
    public class QueryFactory : IQueryFactory
    {
        private readonly AddonAttributeFromTypeQuery _attributeQuery;
        private readonly StartupSceneFromGameSceneQuery _startupSceneConverter;
        private readonly CurrentGameSceneProvider _currentGameSceneProvider;



        public QueryFactory()
        {
            _attributeQuery = new AddonAttributeFromTypeQuery();
            _startupSceneConverter = new StartupSceneFromGameSceneQuery();
            _currentGameSceneProvider = new CurrentGameSceneProvider();
        }



        public IAddonsFromAssemblyQuery GetAddonsFromAssemblyQuery()
        {
            return new AddonsFromAssemblyQuery(_attributeQuery);
        }



        public IAddonAttributeFromTypeQuery GetAddonAttributeQuery()
        {
            return new AddonAttributeFromTypeQuery();
        }



        public IStartupSceneFromGameSceneQuery GetStartupSceneFromGameSceneQuery()
        {
            return new StartupSceneFromGameSceneQuery();
        }

    }
}
