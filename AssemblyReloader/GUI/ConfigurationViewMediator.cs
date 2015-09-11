extern alias KSP;
using System;
using ReeperCommon.Serialization.Exceptions;
using ConfigNode = KSP::ConfigNode;
using AssemblyReloader.Config;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using strange.extensions.injector;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
    public class ConfigurationViewMediator : Mediator
    {
        private const string ConfigurationViewNodeName = "ConfigurationView";

        [Inject] public SignalCloseAllWindows CloseAllWindows { get; set; }
        [Inject] public SignalToggleConfigurationView ToggleConfigurationViewSignal { get; set; }
        [Inject] public SignalSaveWindow SaveWindowState { get; set; }
        [Inject] public SignalOnSaveConfiguration SaveConfigurationSignal { get; set; }
        [Inject] public SignalOnLoadConfiguration LoadConfigurationSignal { get; set; }

        [Inject] public ConfigurationView View { get; set; }
        [Inject] public CoreConfiguration CoreConfiguration { get; set; }
        [Inject] public IConfigNodeSerializer Serializer { get; set; }
        [Inject] public ILog Log { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            View.CloseWindow.AddListener(CloseConfigurationWindow);
            CloseAllWindows.AddListener(CloseConfigurationWindow);
            ToggleConfigurationViewSignal.AddListener(OnToggleConfigurationWindow);

            SaveConfigurationSignal.AddListener(OnSaveConfiguration);
            LoadConfigurationSignal.AddListener(OnLoadConfiguration);

            View.Visible = false; // initially start closed
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.CloseWindow.RemoveListener(CloseConfigurationWindow);
            CloseAllWindows.RemoveListener(CloseConfigurationWindow);
            ToggleConfigurationViewSignal.RemoveListener(OnToggleConfigurationWindow);

            SaveConfigurationSignal.RemoveListener(OnSaveConfiguration);
            LoadConfigurationSignal.RemoveListener(OnLoadConfiguration);
        }


        private void CloseConfigurationWindow()
        {
            Log.Debug("Received close configuration window");

            View.Visible = false;
            SaveWindowState.Dispatch(View);
        }


        private void OnToggleConfigurationWindow()
        {
            View.Visible = !View.Visible;

            if (!View.Visible)
                SaveWindowState.Dispatch(View);
        }


        private void OnSaveConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                Log.Debug("Saving ConfigurationView settings...");
                Serializer.Serialize(View, config.AddNode(ConfigurationViewNodeName));
                Log.Debug("Settings saved");
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed due to serialization exception: " + rse);
            }
        }


        private void OnLoadConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                Log.Normal("Loading ConfigurationView settings...");
                Serializer.Deserialize(View, config.GetNode(ConfigurationViewNodeName));
                Log.Normal("Settings loaded");
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed due to serialization exception: " + rse);
            }
        }
    }
}
