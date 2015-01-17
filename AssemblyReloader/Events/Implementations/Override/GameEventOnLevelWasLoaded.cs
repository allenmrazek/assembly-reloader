//using System;
//using AssemblyReloader.Factory.Implementations;
//using AssemblyReloader.Providers;
//using ReeperCommon.Extensions;
//using ReeperCommon.Logging;
//using UnityEngine;

//namespace AssemblyReloader.Events.Implementations.Override
//{
//    // KSP's OnLevelWasLoaded event triggers twice in some conditions, so this is a workaround
//    // utilizing Unity3d's MonoBehaviour.OnLevelWasLoaded to trigger it instead
//    class GameEventOnLevelWasLoaded : GameEventSubscriber<GameScenes>
//    {
//        private readonly GameObject _levelLoadedView;

//        private class LevelWasLoadedSentinel : MonoBehaviour
//        {
//            public GameEventOnLevelWasLoaded Owner;

//            private void OnLevelWasLoaded(int level)
//            {
//                if (Owner.IsNull())
//                    throw new ArgumentException("Wasn't supplied with Owner");

//                _callback(_gameSceneProvider.Get());
//            }
//        }


//        public GameEventOnLevelWasLoaded(CurrentGameSceneProvider sceneProvider, ILog log) : base(log)
//        {
//            if (sceneProvider == null) throw new ArgumentNullException("sceneProvider");



//            _levelLoadedView = new GameObject("GameEventOnLevelWasLoaded.Proxy");
//            var sentinel = _levelLoadedView.AddComponent<LevelWasLoadedSentinel>();
//            sentinel.

//            UnityEngine.Object.DontDestroyOnLoad(_levelLoadedView);
//        }

//        public override void SubscribeTo(EventData<GameScenes> evt)
//        {
//            // do nothing
//        }

//        public override void UnsubscribeTo(EventData<GameScenes> evt)
//        {
//            // do nothing
//        }
//    }
//}
