using System;
using System.Collections;
using AssemblyReloader.Annotations;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class CoreView : MonoBehaviour
    {
        [UsedImplicitly] private Core _core;

        [UsedImplicitly]
        IEnumerator Start()
        {
            yield return new WaitForSeconds(0f);

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
            //_core.Tick();
        }
    }
}
