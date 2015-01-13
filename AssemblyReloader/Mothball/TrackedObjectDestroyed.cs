using AssemblyReloader.Messages;
using AssemblyReloader.MonoBehaviours;

namespace AssemblyReloader.Tracking.Messages
{
    class TrackedObjectDestroyed : TrackedObjectCreated, ITrackedItemMessage
    {
        public TrackedObjectDestroyed(TrackLifetime source, TrackedItemType type) : base(source, type)
        {
        }
    }
}
