using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.MonoBehaviours;

namespace AssemblyReloader.Messages
{
    interface ITrackedItemMessage : IMessage
    {
        TrackLifetime Source { get; }
        TrackedItemType Type { get; }
    }
}
