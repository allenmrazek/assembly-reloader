using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;
using ReeperCommon.Logging;

namespace AssemblyReloader.Providers
{
    class GameEventProvider : IDisposable
    {
        private readonly ILog _log;

        private readonly IGameEventSource<LevelWasLoadedDelegate> _levelLoadedSource;


        public GameEventProvider(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
            _levelLoadedSource = new GameEventLevelWasLoaded(_log);

            SubscribeProxiesToGameEvents();
        }


        ~GameEventProvider()
        {
            Dispose();
        }


        public IGameEventSource<LevelWasLoadedDelegate> GetLevelLoadedEvent()
        {
            return _levelLoadedSource;
        }

        
        public void Dispose()
        {
            UnsubscribeProxiesToGameEvents();
            GC.SuppressFinalize(this);
        }


        private void SubscribeProxiesToGameEvents()
        {
            GameEvents.onLevelWasLoaded.Add((_levelLoadedSource as GameEventLevelWasLoaded).OnLevelWasLoaded);
        }

        private void UnsubscribeProxiesToGameEvents()
        {
            GameEvents.onLevelWasLoaded.Remove((_levelLoadedSource as GameEventLevelWasLoaded).OnLevelWasLoaded);
        }
    }
}
