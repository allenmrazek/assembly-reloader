using System;
using System.Collections;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using UnityEngine;

namespace AssemblyReloader.Config
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
// ReSharper disable once UnusedMember.Global
    class CoreBootstrapper : ContextView
    {
        private IEnumerator Start()
        {
            enabled = false;
            yield return 0;
            //yield return new WaitForSeconds(10f);
            enabled = true;

            DontDestroyOnLoad(this);

            try
            {
                context = new CoreContext(this);
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
    }
}
