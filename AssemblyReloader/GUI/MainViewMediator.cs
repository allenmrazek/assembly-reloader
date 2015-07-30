using System;
using System.Collections.Generic;
using AssemblyReloader.ReloadablePlugin;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.Gui
{
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
    public class MainViewMediator : Mediator
    {
        [Inject] public MainView View { get; set; }
        [Inject] public SignalCloseAllWindows CloseAllWindowsSignal { get; set; }
        [Inject] public IDictionary<IPluginInfo, IReloadablePlugin> Plugins { get; set; }
        [Inject] public ILog Log { get; set; }

        //public MainViewMediator(MainView view, SignalCloseAllWindows closeAllWindowsSignal,
        //    IDictionary<IPluginInfo, IReloadablePlugin> plugins, ILog log)
        //{
            
        //}

        public override void OnRegister()
        {
            base.OnRegister();

            View.Plugins = Plugins.Keys;

            View.ReloadRequested.AddListener(ReloadRequested);
            View.ToggleConfiguration.AddListener(ToggleConfigurationWindow);
            View.TogglePluginOptions.AddListener(TogglePluginOptions);
            View.Close.AddListener(CloseWindow);

            CloseAllWindowsSignal.AddListener(OnCloseAllWindows);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.ReloadRequested.RemoveListener(ReloadRequested);
            View.ToggleConfiguration.RemoveListener(ToggleConfigurationWindow);
            View.TogglePluginOptions.RemoveListener(TogglePluginOptions);
            View.Close.RemoveListener(CloseWindow);

            CloseAllWindowsSignal.RemoveListener(OnCloseAllWindows);
        }


        private void ReloadRequested(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            Log.Debug("Reload request for {0} received", plugin.Name);
            var reloadable = GetReloadableFromInfo(plugin);
            try
            {
                reloadable.Reload();
            }
            catch (Exception e)
            {
                Log.Error("Encountered unhandled exception while reloading " + plugin.Name + ":" + e);
                // todo: popup message explaining to the user that something got hosed
            }
        }


        private void ToggleConfigurationWindow()
        {
            Log.Debug("Toggle configuration window received");

            //ConfigurationView.Visible = !ConfigurationView.Visible;
        }


        private void TogglePluginOptions(IPluginInfo plugin)
        {
            Log.Debug("Toggle plugin options for {0} received", plugin.Name);

        }


        private void CloseWindow()
        {
            // we should close all open windows if the main window is closed
            CloseAllWindowsSignal.Dispatch();
        }


        public void OnCloseAllWindows()
        {
            View.Visible = false;
        }


        private IReloadablePlugin GetReloadableFromInfo(IPluginInfo info)
        {
            if (info == null) throw new ArgumentNullException("info");

            IReloadablePlugin plugin;

            return Plugins.TryGetValue(info, out plugin) ? plugin : null;
        }
    }
}
