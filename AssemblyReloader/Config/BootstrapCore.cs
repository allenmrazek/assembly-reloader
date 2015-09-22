extern alias KSP;
using System;
using System.Collections;
using ReeperLoader;
using strange.extensions.context.impl;
using UnityEngine;
using HighLogic = KSP::HighLogic;
using PopupDialog = KSP::PopupDialog;

namespace AssemblyReloader.Config
{
    /// <summary>
    /// Note: Uses ReeperLoader because Mono.Cecil 0.9.5 doesn't have support for a particular method
    /// used for reading debug symbols
    /// </summary>
    [ReeperLoadTarget]
// ReSharper disable once UnusedMember.Global
    public class BootstrapCore : ContextView
    {
        private IEnumerator Start()
        {
            // avoid duplicates if GameDatabase is reloaded
            // todo: update GameDatabase urls with new data
            if (FindObjectsOfType<BootstrapCore>().Length > 1)
            {
                print("AssemblyReloader.BootstrapCore - already exists. Exiting");
                Destroy(gameObject);
                yield return null;
            }

            DontDestroyOnLoad(this);
            enabled = false;
            //yield return new WaitForSeconds(10f); // uncomment this when debugging on startup and need time to attach debugger
            enabled = true;


            try
            {
                context = new CoreContext(this);
                context.Start();
                context.Launch();
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating core context: " + e);

                PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
                    "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
                    true, HighLogic.Skin);

                Destroy(gameObject);
            }

            yield return 0;
        }
    }
}
