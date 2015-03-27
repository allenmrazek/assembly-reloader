﻿using System;
using AssemblyReloader.Commands;

namespace AssemblyReloader.Controllers
{

    public class ReloadablePluginController : IReloadablePluginController
    {
        private readonly ICommand _toggleConfigView;

        public ReloadablePluginController(IReloadablePlugin plugin, ICommand toggleConfigView)
        {
            _toggleConfigView = toggleConfigView;
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (toggleConfigView == null) throw new ArgumentNullException("toggleConfigView");

            Plugin = plugin;
        }


        public void Reload()
        {
            Plugin.Unload();
            Plugin.Load();
        }


        public void ToggleConfigurationView()
        {
            _toggleConfigView.Execute();
        }


        public IReloadablePlugin Plugin { get; private set; }
    }
}