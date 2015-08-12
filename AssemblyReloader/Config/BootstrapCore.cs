﻿extern alias KSP;
using System;
using System.Collections;
using System.Linq;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
//using ReeperCommon.Logging;
using UnityEngine;
using KSPAddon = KSP::KSPAddon;
using HighLogic = KSP::HighLogic;
using PopupDialog = KSP::PopupDialog;

namespace AssemblyReloader.Config
{
    [KSPAddon(KSPAddon.Startup.Instantly, true)]
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
            if (FindObjectsOfType<BootstrapCore>().Count() > 1)
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

                Destroy(this);
            }

            yield return 0;
        }
    }
}
