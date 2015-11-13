extern alias KSP;
using System;
using System.Collections;
using System.Linq;
using AssemblyReloader.Game;
using ReeperCommon.Containers;
using ReeperLoader;
using strange.extensions.context.impl;
using UnityEngine;
using AssemblyLoader = KSP::AssemblyLoader;
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
            if (null != FindObjectsOfType<BootstrapCore>()
                .SingleOrDefault(c => !ReferenceEquals(c, this))
                .Do(otherCore =>
            {
                print("AssemblyReloader.BootstrapCore - already exists. Dispatching GameDatabase reload event and exiting");
                try
                {
                    ((CoreContext) otherCore.context).injectionBinder.GetInstance<SignalGameDatabaseReloadTriggered>()
                        .Dispatch();
                }
                catch (Exception e)
                {
                    Debug.LogError("Failed to dispatch SignalGameDatabaseReloadTriggered");
                    Debug.LogException(e);
                }
            }))
            {
                Destroy(gameObject);
                yield break;
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
