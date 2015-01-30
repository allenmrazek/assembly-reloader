using System;

namespace AssemblyReloader.Queries.Implementations
{
    class CurrentStartupSceneQuery : ICurrentKSPStartupSceneQuery
    {
        private readonly StartupSceneFromGameSceneQuery _query;

        public CurrentStartupSceneQuery(StartupSceneFromGameSceneQuery query)
        {
            if (query == null) throw new ArgumentNullException("query");
            _query = query;
        }


        public KSPAddon.Startup Get()
        {
            return _query.Get(HighLogic.LoadedScene);
        }
    }
}
