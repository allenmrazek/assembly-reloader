using System;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IQueryScenarioModulesCanRunInCurrentScene), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class QueryScenarioModulesCanRunInCurrentScene : IQueryScenarioModulesCanRunInCurrentScene
    {
        private static readonly GameScenes[] ValidScenes =
        {
            GameScenes.SPACECENTER,
            GameScenes.FLIGHT,
            GameScenes.SPACECENTER, 
            GameScenes.TRACKSTATION,
            GameScenes.EDITOR
        };

        private readonly IGetCurrentGameScene _gameSceneQuery;

        public QueryScenarioModulesCanRunInCurrentScene(IGetCurrentGameScene gameSceneQuery)
        {
            if (gameSceneQuery == null) throw new ArgumentNullException("gameSceneQuery");
            _gameSceneQuery = gameSceneQuery;
        }


        public bool Get()
        {
            return ValidScenes.Contains(_gameSceneQuery.Get());
        }
    }
}
