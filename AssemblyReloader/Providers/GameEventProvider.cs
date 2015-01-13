using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Events;
using AssemblyReloader.Events.Implementations;

namespace AssemblyReloader.Providers
{
    class GameEventProvider : IDisposable
    {
        private readonly IGameEventSource<LevelWasLoadedDelegate> _levelLoadedSource;


        public GameEventProvider()
        {
            _levelLoadedSource = new GameEventLevelWasLoaded();
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
            _levelLoadedSource.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
