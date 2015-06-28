using System;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Queries;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    [Implements(typeof(IGetCurrentStartupScene))]
// ReSharper disable once UnusedMember.Global
    public class GetCurrentStartupScene : IGetCurrentStartupScene
    {
        private readonly IGetStartupSceneFromGameScene _query;
        private readonly ICurrentGameSceneProvider _currentGameScene;

        public GetCurrentStartupScene(IGetStartupSceneFromGameScene query, ICurrentGameSceneProvider currentGameScene)
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
