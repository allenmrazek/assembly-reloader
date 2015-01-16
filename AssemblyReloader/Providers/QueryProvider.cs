using System;
using System.Reflection;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Providers
{
    class QueryProvider
    {
        private readonly AddonAttributeFromTypeQuery _attributeQuery;
        private readonly StartupSceneFromGameSceneQuery _startupSceneConverter;
        private readonly CurrentGameSceneProvider _currentGameSceneProvider;


        public QueryProvider()
        {
            _attributeQuery = new AddonAttributeFromTypeQuery();
            _startupSceneConverter = new StartupSceneFromGameSceneQuery();
            _currentGameSceneProvider = new CurrentGameSceneProvider();
        }



        public AddonsFromAssemblyQuery GetAddonsFromAssemblyQuery(Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");

            return new AddonsFromAssemblyQuery(assembly, _attributeQuery);
        }



        public AddonAttributeFromTypeQuery GetAddonAttributeQuery()
        {
            return _attributeQuery;
        }


        public StartupSceneFromGameSceneQuery GetStartupSceneFromGameSceneQuery()
        {
            return _startupSceneConverter;
        }

        public CurrentGameSceneProvider GetCurrentGameSceneProvider()
        {
            return _currentGameSceneProvider;
        }
    }
}
