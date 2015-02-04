using System;
using AssemblyReloader.Queries.ConversionQueries;

namespace AssemblyReloader.Providers
{
    class CurrentStartupSceneProvider : ICurrentKSPStartupSceneProvider
    {
        private readonly StartupSceneFromGameSceneQuery _query;

        public CurrentStartupSceneProvider(StartupSceneFromGameSceneQuery query)
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
