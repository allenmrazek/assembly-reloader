using System;
using AssemblyReloader.AddonTracking;
using UnityEngine;

namespace AssemblyReloader.Messages.Implementation
{
    class AddonCreated
    {
        private readonly GameObject _created;
        private readonly AddonLifetimeTracker _source;

        public AddonCreated(GameObject created, AddonLifetimeTracker source)
        {
            if (created == null) throw new ArgumentNullException("created");
            if (source == null) throw new ArgumentNullException("source");
            _created = created;
            _source = source;
        }

        public GameObject Addon { get { return _created; } }
        public AddonLifetimeTracker Sender { get { return _source; }}
    }
}
