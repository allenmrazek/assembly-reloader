using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Weaving.Operations.GameEventInterception
{
    public class GameEventRegistry : IGameEventRegistry
    {
        private readonly IDictionary<GameEventReference, List<GameEventCallback>> _registrations =
            new Dictionary<GameEventReference, List<GameEventCallback>>();


        public void Add(GameEventReference gameEvent, GameEventCallback callback)
        {
            if (gameEvent == null) throw new ArgumentNullException("gameEvent");
            if (callback == null) throw new ArgumentNullException("callback");

            // note to self: duplicate registrations allowed (probably an error from the caller
            // but not our business)
            GetCallbackList(gameEvent).Add(callback);
        }


        public bool Remove(GameEventReference gameEvent, GameEventCallback callback)
        {
            if (gameEvent == null) throw new ArgumentNullException("gameEvent");
            if (callback == null) throw new ArgumentNullException("callback");

            var log = new DebugLog("GameEventRegistryTemp");

            log.Normal("Have callback list? " + (GetCallbackList(gameEvent) == null));
            log.Normal("Looking for " + gameEvent);
            log.Normal("Have " + _registrations.Keys.ToArray().Length + " game event lists");
            foreach (var k in _registrations.Keys)
                log.Normal("List for: " + k + " with " + _registrations[k].Count + " entries");

            log.Normal("Found? " + GetCallbackList(gameEvent)
                .Return(list => list.Contains(callback), false));

            return GetCallbackList(gameEvent).Remove(callback);
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

        IEnumerator<GameEventCallback> IEnumerable<GameEventCallback>.GetEnumerator()
        {
            return _registrations.SelectMany(kvp => kvp.Value).GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _registrations.SelectMany(kvp => kvp.Value).GetEnumerator();
        }
    }
}
