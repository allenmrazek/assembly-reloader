using AssemblyReloader.AddonTracking;
using UnityEngine;

namespace AssemblyReloader.Messages.Implementation
{
    class AddonDestroyed : AddonCreated
    {
        public AddonDestroyed(GameObject destroyed, AddonLifetimeTracker source)
            : base(destroyed, source)
        {
        }
    }
}
