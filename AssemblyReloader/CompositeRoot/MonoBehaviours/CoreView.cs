using System;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot.MonoBehaviours
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
// ReSharper disable once UnusedMember.Global
    class CoreView : MonoBehaviour
    {
        private Core _core;

// ReSharper disable once UnusedMember.Local
        private void Start()
        {
            try
            {
                _core = new Core();
                DontDestroyOnLoad(this);
            }
            catch (Exception e)
            {
                print("CoreView: Encountered an uncaught exception while creating Core: " + e);
                Destroy(this);
            }
        }
    }
}
