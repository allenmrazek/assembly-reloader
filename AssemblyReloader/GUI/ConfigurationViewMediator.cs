using System;
using ReeperCommon.Serialization.Exceptions;
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
        [Inject] public SignalOnSaveConfiguration SaveConfigurationSignal { get; set; }
        [Inject] public SignalOnLoadConfiguration LoadConfigurationSignal { get; set; }
        [Inject] public SignalInterfaceScaleChanged InterfaceScaleChangedSignal { get; set; }

        private ConfigurationView _view;
        [Inject] public ConfigurationView View { get { return _view; } set { _view = value; } }

        [Inject] public CoreConfiguration CoreConfiguration { get; set; }
        [Inject] public IConfigNodeSerializer Serializer { get; set; }
        [Inject] public ILog Log { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            View.CloseWindow.AddListener(CloseConfigurationWindow);

            CloseAllWindows.AddListener(CloseConfigurationWindow);
            ToggleConfigurationViewSignal.AddListener(OnToggleConfigurationWindow);
            InterfaceScaleChangedSignal.AddListener(OnInterfaceScaleChanged);

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


        private void OnInterfaceScaleChanged(float f)
        {
            View.SetScale(f);
        }


        private void CloseConfigurationWindow()
        {
            View.Visible = false;
        }


        private void OnToggleConfigurationWindow()
        {
            View.Visible = !View.Visible;
        }


        private void OnSaveConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                Log.Debug("Saving ConfigurationView settings...");
                var serialized = Serializer.CreateConfigNodeFromObject(View);
                serialized.name = ConfigurationViewNodeName;

                config.AddNode(serialized);

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
                Serializer.LoadObjectFromConfigNode(ref _view, config.GetNode(ConfigurationViewNodeName));
                Log.Normal("Settings loaded");
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed due to serialization exception: " + rse);
            }
        }
    }
}
