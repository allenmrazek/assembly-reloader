using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Events.Implementations
{
    class GameEventSubscription<T> : IGameEventSubscription
    {
        private readonly IGameEventSource<T> _src;
        private readonly T _callback;



        public GameEventSubscription(IGameEventSource<T> src, T callback)
        {
            if (src == null) throw new ArgumentNullException("src");

            _src = src;
            _callback = callback;
        }



        public void Dispose()
        {
            Dispose(true);
        }



        ~GameEventSubscription()
        {
            Dispose(false);
        }



        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _src.Remove(_callback);
            }
        }
    }
}
