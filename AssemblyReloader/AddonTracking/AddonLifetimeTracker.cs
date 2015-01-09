using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Loaders;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.AddonTracking
{
    class AddonLifetimeTracker : MonoBehaviour
    {
        public Action OnDestroyed;

        private void OnDestroy()
        {
            if (!OnDestroyed.IsNull())
                OnDestroyed();
        }
    }
}
