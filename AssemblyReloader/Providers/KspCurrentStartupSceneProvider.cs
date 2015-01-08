using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Providers
{
    class KspCurrentStartupSceneProvider
    {
        private readonly KspStartupSceneFromGameSceneQuery _query;

        public KspCurrentStartupSceneProvider(KspStartupSceneFromGameSceneQuery query)
        {
            if (query == null) throw new ArgumentNullException("query");
            _query = query;
        }


        public KSPAddon.Startup Get()
        {
            return _query.Query(HighLogic.LoadedScene);
        }
    }
}
