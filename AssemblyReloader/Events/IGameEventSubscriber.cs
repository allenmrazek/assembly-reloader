using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.Events
{
    public interface IGameEventSubscriber<T> : IDisposable
    {
        IGameEventSubscription AddListener(Action<T> callback);
        void RemoveListener(Action<T> callback);

        void OnEvent(T arg);

        void SubscribeTo(EventData<T> evt);
        void UnsubscribeTo(EventData<T> evt);
    }
}
