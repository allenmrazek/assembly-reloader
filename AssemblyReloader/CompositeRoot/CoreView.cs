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
            //yield return new WaitForSeconds(8f);

            yield return 0;

            try
            {
                _core = new Core();
                DontDestroyOnLoad(this);
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating Core: " + e);

                PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
                    "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
                    true, HighLogic.Skin);
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
