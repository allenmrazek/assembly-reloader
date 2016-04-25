using System;
using System.Collections.Generic;
using AssemblyReloader.ReloadablePlugin;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperKSP.Serialization;
using ReeperKSP.Serialization.Exceptions;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Gui
{
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
    public class MainViewMediator : Mediator
    {
        private const string MainViewNodeName = "MainView";

        [Inject] public SignalCloseAllWindows CloseAllWindowsSignal { get; set; }
        [Inject] public SignalApplicationLauncherButtonToggle AppLauncherToggleSignal { get; set; }
        [Inject] public SignalApplicationLauncherButtonCreated AppLauncherButtonCreatedSignal { get; set; }
        [Inject] public SignalInterfaceScaleChanged InterfaceScaleChangedSignal { get; set; }

        [Inject] public SignalTogglePluginConfigurationView TogglePluginOptionsSignal { get; set; }
        [Inject] public SignalToggleConfigurationView ToggleConfigurationOptionsSignal { get; set; }
        [Inject] public SignalOnSaveConfiguration SaveConfigurationSignal { get; set; }
        [Inject] public SignalOnLoadConfiguration LoadConfigurationSignal { get; set; }
        [Inject] public SignalMainViewVisibilityChanged ViewVisibilityChangedSignal { get; set; }
        



        private MainView _view;

        [Inject] // intended: can't ref auto-implemented accessors
        public MainView View
        {
            get { return _view; }
            set { _view = value; }
        }

        [Inject] public IDictionary<IPluginInfo, IReloadablePlugin> Plugins { get; set; }

        [Inject] public IConfigNodeSerializer Serializer { get; set; }
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

            SaveConfigurationSignal.AddListener(OnSaveConfiguration);
            LoadConfigurationSignal.AddListener(OnLoadConfiguration);

            AppLauncherToggleSignal.AddListener(OnAppButtonToggle);
            InterfaceScaleChangedSignal.AddListener(OnInterfaceScaleChanged);
            AppLauncherButtonCreatedSignal.AddOnce(OnAppLauncherButtonCreated);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.ReloadRequested.RemoveListener(ReloadRequested);
            View.ToggleConfiguration.RemoveListener(ToggleConfigurationWindow);
            View.TogglePluginConfiguration.RemoveListener(TogglePluginOptions);
            View.Close.RemoveListener(CloseWindow);

            CloseAllWindowsSignal.RemoveListener(OnCloseAllWindows);

            SaveConfigurationSignal.RemoveListener(OnSaveConfiguration);
            LoadConfigurationSignal.RemoveListener(OnLoadConfiguration);

            AppLauncherToggleSignal.RemoveListener(OnAppButtonToggle);
            InterfaceScaleChangedSignal.RemoveListener(OnInterfaceScaleChanged);
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
            DispatchVisibilityState();
        }


        private void OnCloseAllWindows()
        {
            View.Visible = false;
            DispatchVisibilityState();
        }


        private IReloadablePlugin GetReloadableFromInfo(IPluginInfo info)
        {
            if (info == null) throw new ArgumentNullException("info");

            IReloadablePlugin plugin;

            return Plugins.TryGetValue(info, out plugin) ? plugin : null;
        }


        private void OnSaveConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                Log.Debug("Serializing MainView settings...");
                Serializer.WriteObjectToConfigNode(ref _view, config.AddNode("MainView"));
                Log.Debug("MainView settings serialized");
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed due to exception: " + rse);
            }
        }


        private void OnLoadConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                config.GetNode(MainViewNodeName).Do(n =>
                {
                    Log.Debug("Deserializing MainView settings...");
                    Serializer.LoadObjectFromConfigNode(ref _view, n);
                    Log.Debug("MainView settings loaded");
                })
                .IfNull(() => Log.Warning("No MainView ConfigNode found; using default"));

                DispatchVisibilityState(); // dispatch event to sync AppButton toggle status with main window visibility
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed due to exception: " + rse);
            }
        }


        private void OnAppLauncherButtonCreated()
        {
            DispatchVisibilityState(); // sync AppLauncher button toggle status up with current state
        }


        private void OnAppButtonToggle(bool b)
        {
            if (!b)
            {
                CloseWindow(); // call instead of just setting because this is tied to more actions (closing all windows)
            }
            else View.Visible = true;

            DispatchVisibilityState();
        }


        private void DispatchVisibilityState()
        {
            ViewVisibilityChangedSignal.Dispatch(View.Visible);
        }


        private void OnInterfaceScaleChanged(float f)
        {
            View.SetScale(f);
        }

    }
}
