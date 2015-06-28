using System;
using System.Collections;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using UnityEngine;

namespace AssemblyReloader.CompositeRoot
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
    class CoreView : ContextView
    {
        private IEnumerator Start()
        {
            enabled = false;
            yield return 0;
            enabled = true;

            DontDestroyOnLoad(this);

            try
            {
                context = new CoreContext(this, true);
                context.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating core context: " + e);

                PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
                    "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
                    true, HighLogic.Skin);

                Destroy(this);
            }
        }


        //private void Update()
        //{
            
        //}
    }

    //[KSPAddon(KSPAddon.Startup.Instantly, true)]
    //class CoreView : MonoBehaviour
    //{
    //    private static bool _initialized = false; // this is to cover cases where database is reloaded and AssemblyReloader already exists
    //    [UsedImplicitly]
    //    private Core _core;

    //    [UsedImplicitly]
    //    IEnumerator Start()
    //    {
    //        enabled = false;
    //        yield return 0;
    //        enabled = true;

    //        if (_initialized)
    //        {
    //            Debug.LogWarning("AssemblyReloader already initialized");
    //            yield break;
    //        }

    //        try
    //        {
    //            _core = new Core();
    //            DontDestroyOnLoad(this);
    //            _initialized = true;
    //        }
    //        catch (Exception e)
    //        {
    //            Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating Core: " + e);

    //            PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
    //                "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
    //                true, HighLogic.Skin);

    //            Destroy(this);
    //        }
    //    }


    //    [UsedImplicitly]
    //    private void Update()
    //    {
    //        _core.Tick();
    //    }
    //}
}
