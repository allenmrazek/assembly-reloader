using System;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Queries;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public class GetCurrentStartupScene : IGetCurrentStartupScene
    {
        private readonly IStartupSceneFromGameSceneQuery _query;
        private readonly ICurrentGameSceneProvider _currentGameScene;

        public GetCurrentStartupScene(IStartupSceneFromGameSceneQuery query, ICurrentGameSceneProvider currentGameScene)
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
