using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Events
{
    interface IGameEventSource<T> : IDisposable
    {
        IGameEventSubscription Add(T callback);
        void Remove(T callback);
    }
}
