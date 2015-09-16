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
    [ReeperLoadTarget]
// ReSharper disable once UnusedMember.Global
    public class BootstrapCore : ContextView
    {
        //private static Logger logger = LogManager.GetCurrentClassLogger();

        private IEnumerator Start()
        {
            //logger.Debug("Boostrapping...");

            //new DebugLog("Test").Normal("Test log output");

            // avoid duplicates if GameDatabase is reloaded
            // todo: update GameDatabase urls with new data
            if (FindObjectsOfType<BootstrapCore>().Length > 1)
            {
                Destroy(gameObject);
                yield return null;
            }

            DontDestroyOnLoad(this);
            enabled = false;
            yield return new WaitForSeconds(10f);
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
