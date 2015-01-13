using System;

namespace AssemblyReloader.Events.Implementations
{
    public delegate void LevelWasLoadedDelegate(GameScenes scene);



    class GameEventLevelWasLoaded : IGameEventSource<LevelWasLoadedDelegate>
    {
        private event LevelWasLoadedDelegate SceneChanged = delegate { };



        public GameEventLevelWasLoaded() { GameEvents.onLevelWasLoaded.Add(OnLevelWasLoaded); }
       
        public IGameEventSubscription Add(LevelWasLoadedDelegate callback)
        {
            var subscriber = new GameEventSubscription<LevelWasLoadedDelegate>(this, callback);
            SceneChanged += callback;

            return subscriber;
        }

        public void Remove(LevelWasLoadedDelegate callback)
        {
            SceneChanged -= callback;
        }

        public void Dispose()
        {
            GameEvents.onLevelWasLoaded.Add(OnLevelWasLoaded);
        }

        private void OnLevelWasLoaded(GameScenes scene)
        {
            SceneChanged(scene);
        }
    }
}
