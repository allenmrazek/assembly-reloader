using System;
using AssemblyReloader.Messages;
using AssemblyReloader.MonoBehaviours;

namespace AssemblyReloader.Tracking.Messages
{
    class TrackedObjectCreated : ITrackedItemMessage
    {
        public TrackedObjectCreated(TrackLifetime source, TrackedItemType type)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
            Type = type;
        }

        public TrackLifetime Source { get; private set; }
        public TrackedItemType Type { get; private set; }
    }
}
