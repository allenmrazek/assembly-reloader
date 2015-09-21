using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GameEventRegistry : IGameEventRegistry 
    {
        private const string GameEventAddMethodName = "Add";
        private const string GameEventRemoveMethodName = "Remove";

        private readonly ILog _log;

        private readonly IDictionary<GameEventReference, List<GameEventCallback>> _registrations =
            new Dictionary<GameEventReference, List<GameEventCallback>>();

        public GameEventRegistry(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
        }


        public void Add(GameEventReference gameEvent, GameEventCallback callback)
        {
            if (gameEvent == null) throw new ArgumentNullException("gameEvent");
            if (callback == null) throw new ArgumentNullException("callback");

            try
            {
                // note to self: duplicate registrations allowed (probably an error from the caller
                // but not our business)
                InvokeMethod(gameEvent, GameEventAddMethodName, callback);
                GetCallbackList(gameEvent).Add(callback);
            }
            catch (Exception e)
            {
                _log.Error("Exception adding game event to registry: " + e);
            }
        }


        public bool Remove(GameEventReference gameEvent, GameEventCallback callback)
        {
            if (gameEvent == null) throw new ArgumentNullException("gameEvent");
            if (callback == null) throw new ArgumentNullException("callback");

            try
            {
                // note to self: duplicate registrations allowed (probably an error from the caller
                // but not our business)
                bool result = GetCallbackList(gameEvent).Remove(callback);
                InvokeMethod(gameEvent, GameEventRemoveMethodName, callback);

                return result;
            }
            catch (Exception e)
            {
                _log.Error("Exception removing game event from registry: " + e);
            }

            return false;
        }


        public void ClearCallbacks()
        {
            var keys = _registrations.Keys.ToArray();

            foreach (var gameEvent in keys)
            {
                var callbacksForThisEvent = _registrations[gameEvent];

                while (callbacksForThisEvent.Any())
                    Remove(gameEvent, callbacksForThisEvent.First());
            }

            _registrations.Clear();
        }


        public int Count
        {
            get { return _registrations.SelectMany(r => r.Value).Count(); }
        }


        private List<GameEventCallback> GetCallbackList(GameEventReference gameEvent)
        {
            List<GameEventCallback> result;

            if (_registrations.TryGetValue(gameEvent, out result)) return result;

            result = new List<GameEventCallback>();
            _registrations.Add(gameEvent, result);

            return result;
        }


        private void InvokeMethod(GameEventReference gameEvent, string methodName, GameEventCallback callback)
        {
            gameEvent.GameEventRef.GetType()
                .GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance)
                .Do(mi => mi.Invoke(gameEvent.GameEventRef, new object[] {callback.CallbackDelegate}));
        }

        IEnumerator<KeyValuePair<GameEventReference, GameEventCallback>> IEnumerable<KeyValuePair<GameEventReference, GameEventCallback>>.GetEnumerator()
        {
            return _registrations
                .SelectMany(kvp => kvp.Value
                    .Select(cb => new KeyValuePair<GameEventReference, GameEventCallback>(kvp.Key, cb)))
                    .GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _registrations.SelectMany(kvp => kvp.Value).GetEnumerator();
        }
    }
}
