using System;
using System.Linq;
using ReeperCommon.Logging;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class MonoBehaviourDestroyer : IMonoBehaviourDestroyer
    {
        private readonly SignalAboutToDestroyMonoBehaviour _mbDestructionSignal;
        private readonly ILog _log;

        public MonoBehaviourDestroyer(
            SignalAboutToDestroyMonoBehaviour mbDestructionSignal,
            ILog log)
        {
            if (mbDestructionSignal == null) throw new ArgumentNullException("mbDestructionSignal");
            if (log == null) throw new ArgumentNullException("log");

            _mbDestructionSignal = mbDestructionSignal;
            _log = log;
        }


        public void Destroy(MonoBehaviour target)
        {
            if (target == null)
                return; // Unity overloads this operator to check for existing of unmanaged side

            _mbDestructionSignal.Dispatch(target);

            Object.Destroy(target);
        }
    }
}
