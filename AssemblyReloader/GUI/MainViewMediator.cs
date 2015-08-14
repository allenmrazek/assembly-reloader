using System;
using System.Collections.Generic;
using AssemblyReloader.ReloadablePlugin;
using ReeperCommon.Logging;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Gui
{
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
    public class MainViewMediator : Mediator
    {
        [Inject] public SignalCloseAllWindows CloseAllWindowsSignal { get; set; }
        [Inject] public SignalTogglePluginConfigurationView TogglePluginOptionsSignal { get; set; }
        [Inject] public SignalToggleConfigurationView ToggleConfigurationOptionsSignal { get; set; }

        [Inject] public MainView View { get; set; }
        [Inject] public IDictionary<IPluginInfo, IReloadablePlugin> Plugins { get; set; }
        [Inject] public ILog Log { get; set; }


        public override void OnRegister()
        {
            base.OnRegister();

            View.Plugins = Plugins.Keys;

            View.ReloadRequested.AddListener(ReloadRequested);
            View.ToggleConfiguration.AddListener(ToggleConfigurationWindow);
            View.TogglePluginConfiguration.AddListener(TogglePluginOptions);
            View.Close.AddListener(CloseWindow);

            CloseAllWindowsSignal.AddListener(OnCloseAllWindows);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.ReloadRequested.RemoveListener(ReloadRequested);
            View.ToggleConfiguration.RemoveListener(ToggleConfigurationWindow);
            View.TogglePluginConfiguration.RemoveListener(TogglePluginOptions);
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

            ToggleConfigurationOptionsSignal.Dispatch();
        }


        private void TogglePluginOptions(IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            Log.Debug("Toggle plugin options for {0} received", plugin.Name);

            TogglePluginOptionsSignal.Dispatch(plugin);
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
