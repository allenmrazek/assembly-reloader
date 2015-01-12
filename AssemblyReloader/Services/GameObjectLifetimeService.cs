using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Messages;
using AssemblyReloader.MonoBehaviours;
using AssemblyReloader.Tracking.Messages;
using UnityEngine;

namespace AssemblyReloader.Services
{
    class GameObjectLifetimeService
    {
        private readonly IChannel _messageChannel;

        public GameObjectLifetimeService(IChannel messageChannel)
        {
            if (messageChannel == null) throw new ArgumentNullException("messageChannel");
            _messageChannel = messageChannel;
        }

        public void RegisterAddon(GameObject go)
        {
            var tracker = go.AddComponent<TrackLifetime>();

            tracker.StartMessage = new TrackedObjectCreated(tracker, TrackedItemType.Addon);
            tracker.DestroyMessage = new TrackedObjectDestroyed(tracker, TrackedItemType.Addon);
            tracker.Channel = _messageChannel;
        }
    }
}
