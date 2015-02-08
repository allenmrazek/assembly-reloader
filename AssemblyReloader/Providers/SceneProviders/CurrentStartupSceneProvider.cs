using System;
using AssemblyReloader.Queries.ConversionQueries;

namespace AssemblyReloader.Providers.SceneProviders
{
    public class CurrentStartupSceneProvider : ICurrentStartupSceneProvider
    {
        private readonly IStartupSceneFromGameSceneQuery _query;
        private readonly ICurrentGameSceneProvider _currentGameScene;

        public CurrentStartupSceneProvider(IStartupSceneFromGameSceneQuery query, ICurrentGameSceneProvider currentGameScene)
        {
            if (query == null) throw new ArgumentNullException("query");
            if (currentGameScene == null) throw new ArgumentNullException("currentGameScene");
            _query = query;
            _currentGameScene = currentGameScene;
        }


        public KSPAddon.Startup Get()
        {
            return _query.Get(_currentGameScene.Get());
        }
    }
}
