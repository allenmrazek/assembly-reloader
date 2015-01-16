using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Events.Implementations
{
    internal class GameEventSubscriber<T> : IGameEventSubscriber<T>
    {

        private readonly ILog _log;
        private WeakReference _subscribed;
        private Action<T> _actions = delegate(T arg) { };


        public GameEventSubscriber(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }



        ~GameEventSubscriber()
        {
            Dispose();
        }



        public void Dispose()
        {
            if (!_subscribed.IsNull() && _subscribed.IsAlive)
            {
                _log.Debug("GameEventSubscriber<" + typeof (T).FullName + "> disposing");
                UnsubscribeTo(_subscribed.Target as EventData<T>);
            }
            GC.SuppressFinalize(this);
        }



        public IGameEventSubscription AddListener(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            _actions += callback;

            return new GameEventSubscription<T>(this, callback);
        }



        public void RemoveListener(Action<T> callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            // ReSharper disable once DelegateSubtraction
            _actions -= callback;
        }



        public void OnEvent(T arg)
        {
            _log.Debug("Triggered with arg " + arg.ToString());
            _actions(arg);
        }



        public virtual void SubscribeTo(EventData<T> evt)
        {
            if (evt == null) throw new ArgumentNullException("evt");
            if (!_subscribed.IsNull())
                throw new InvalidOperationException("Already subscribed to an event");

            _subscribed = new WeakReference(evt);

            evt.Add(OnEvent);
        }



        public virtual void UnsubscribeTo(EventData<T> evt)
        {
            if (_subscribed.IsAlive)
            {
                var publisher = _subscribed.Target as EventData<T>;

                if (!publisher.IsNull())
                    publisher.Remove(OnEvent);
            }
        }
    }
}
