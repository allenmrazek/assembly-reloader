using System;
using AssemblyReloader.Mediators;
using AssemblyReloader.TypeTracking;
using UnityEngine;

namespace AssemblyReloader.Loaders.Addon
{
    // Represents a potentially live KSPAddon that ART has created
    class Addon : IDisposable
    {
        private readonly IDestructionMediator _destructionMediator;
        private readonly MonoBehaviour _monoBehaviour; // do not replace with interface: Unity's operator overloading will not function
                                                       // correctly. It overloads Equals and == to return false from the managed side of
                                                       // the engine when the unmanaged side has been flagged for destruction or was destroyed.
                                                       // An interface here will prevent those overloads from working correctly

        



        public Addon(
            MonoBehaviour monoBehaviour,
            IDestructionMediator destructionMediator)
        {
            if (monoBehaviour == null) throw new ArgumentNullException("monoBehaviour");
            if (destructionMediator == null) throw new ArgumentNullException("destructionMediator");

            _monoBehaviour = monoBehaviour;
            _destructionMediator = destructionMediator;
        }



        ~Addon()
        {
            Dispose();
        }



        public void Dispose()
        {
            if (_monoBehaviour.Equals(null) || _monoBehaviour == null) // Unity overloads these
                return;

            _destructionMediator.InformTargetOfDestruction(_monoBehaviour.gameObject);
            UnityEngine.Object.Destroy(_monoBehaviour.gameObject);

            GC.SuppressFinalize(this);
        }
    }
}
