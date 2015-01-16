using System;
using System.Collections.Generic;
using System.Linq;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Factory.Implementations
{
    // note: made decision not to get too tricky with lifetime tracking: we'll delegate the responsibility 
    // for any advanced cleanup to the assembly being reloaded, which will know much better than us how
    // to deal with it
    class MonoBehaviourFactory : IMonoBehaviourFactory
    {
        private readonly ILog _log;
        private readonly List<MonoBehaviour> _trackedItems = new List<MonoBehaviour>();


        public MonoBehaviourFactory(ILog log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;
        }



        public MonoBehaviour Create(Type type, bool track)
        {
            if (type == null) throw new ArgumentNullException("type");

            if (!typeof (MonoBehaviour).IsAssignableFrom(type))
                throw new ArgumentException("type must be a MonoBehaviour");

            _log.Verbose("Creating " + type.FullName);

            var mb = new GameObject(type.FullName).AddComponent(type) as MonoBehaviour;

            if (track)
                _trackedItems.Add(mb);

            return mb;
        }



        public IEnumerable<MonoBehaviour> GetLiveMonoBehaviours()
        {
            var removed = _trackedItems.RemoveAll(mb => ReferenceEquals(mb, null) || mb.Equals(null));

            _log.Debug(removed + " items no longer tracked because they have been destroyed");

            return _trackedItems.AsEnumerable<MonoBehaviour>();
        }


   
    }
}
