using System;
using ReeperCommon.Logging;

namespace AssemblyReloader.Events.Implementations
{
    public delegate void LevelWasLoadedDelegate(GameScenes scene);



    class GameEventLevelWasLoaded : IGameEventSource<LevelWasLoadedDelegate>
    {
        private readonly ILog _log;
        private event LevelWasLoadedDelegate SceneChanged = delegate { };



        public GameEventLevelWasLoaded(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;

        }

        public IGameEventSubscription Add(LevelWasLoadedDelegate callback)
        {
            _log.Debug("Added new subscriber");

            var subscriber = new GameEventSubscription<LevelWasLoadedDelegate>(this, callback);
            SceneChanged += callback;

            return subscriber;
        }

        public void Remove(LevelWasLoadedDelegate callback)
        {
            _log.Debug("removed subscriber");
            SceneChanged -= callback;
        }


        public void OnLevelWasLoaded(GameScenes scene)
        {
            _log.Debug("level was loaded: {0}", scene.ToString());
            SceneChanged(scene);
        }
    }
}
