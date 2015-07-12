using System;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using ReeperCommon.FileSystem;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin
{
    public class ReloadablePluginContextBootstrap : ContextView
    {
        public void Initialize(IFile reloadableFile)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            try
            {
                context = new ReloadablePluginContext(this, reloadableFile, true);
                context.Start();
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating ReloadablePlugin context: " + e);

                PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
                    "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
                    true, HighLogic.Skin);

                // todo: shut down
            }
        }
    }
}
