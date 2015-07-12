using System;
using System.Collections.Generic;
using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.context.impl;
using ReeperCommon.FileSystem;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ReloadablePluginContextBootstrap : ContextView
    {
        public KeyValuePair<IPluginInfo, IReloadablePlugin> Initialize(IFile reloadableFile)
        {
            if (reloadableFile == null) throw new ArgumentNullException("reloadableFile");

            try
            {
#if DEBUG
                print("Bootstrapping context for " + reloadableFile.Url);
#endif
                var reloadablePluginContext = new ReloadablePluginContext(this, reloadableFile);
                context = reloadablePluginContext;
                context.Start();

                return new KeyValuePair<IPluginInfo, IReloadablePlugin>(reloadablePluginContext.Info, reloadablePluginContext.Plugin);
            }
            catch (Exception e)
            {
                Debug.LogError("AssemblyReloader: Encountered an uncaught exception while creating ReloadablePlugin context for " + reloadableFile.Url + ": " + e);

                PopupDialog.SpawnPopupDialog("AssemblyReloader unhandled exception",
                    "AssemblyReloader encountered an exception with the following message: " + e.Message + "\n\nAssemblyReloader has been disabled.", "Okay",
                    true, HighLogic.Skin);

                throw;
            }
        }
    }
}
