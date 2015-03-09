using System;
using ReeperCommon.Extensions;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot.MonoBehaviours
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class CoreView : MonoBehaviour
    {
        private Core _core;

        private void Start()
        {
            try
            {
                _core = new Core();
                DontDestroyOnLoad(this);
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating Core: " + e);
                Destroy(this);
            }
        }
    }
}
