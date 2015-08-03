﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class MonoBehaviourDestroyer : IMonoBehaviourDestroyer
    {
        private readonly SignalAboutToDestroyMonoBehaviour _mbDestructionSignal;

        public MonoBehaviourDestroyer(SignalAboutToDestroyMonoBehaviour mbDestructionSignal)
        {
            if (mbDestructionSignal == null) throw new ArgumentNullException("mbDestructionSignal");

            _mbDestructionSignal = mbDestructionSignal;
        }


        public void DestroyMonoBehaviour(MonoBehaviour target)
        {
            if (target == null)
                return; // Unity overloads this operator to check for existing of unmanaged side

            _mbDestructionSignal.Dispatch(target);

            Object.Destroy(target);
        }
    }
}