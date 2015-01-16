using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Events
{
    interface IGameEventSource<T>
    {
        IGameEventSubscription Add(T callback);
        void Remove(T callback);

        void Trigger(object arg);
    }
}
