using System;
using AssemblyReloader.Annotations;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class CoreView : MonoBehaviour
    {
        [UsedImplicitly] private Core _core;

        [UsedImplicitly]
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
                DestroyImmediate(this);
            }
        }


        [UsedImplicitly]
        private void Update()
        {
            _core.Tick();
        }
    }
}
