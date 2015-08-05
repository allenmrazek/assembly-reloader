using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PluginConfigurationViewMediator : Mediator
    {
// ReSharper disable MemberCanBePrivate.Global
        [Inject] public PluginConfigurationView View { get; set; }
        [Inject] public ILog Log { get; set; }
        [Inject] public SignalCloseAllWindows CloseAllWindowsSignal { get; set; }
        [Inject] public SignalTogglePluginConfigurationView TogglePluginConfigurationSignal { get; set; }
        [Inject] public IPluginInfo PluginInfo { get; set; }

        private void Awake()
        {
            print("PluginConfigurationViewMediator awake");
        }


        public override void OnRegister()
        {
            Log.Debug("PluginConfigurationViewMediator starting");

            base.OnRegister();

            View.CloseWindow.AddListener(OnCloseWindow);
            CloseAllWindowsSignal.AddListener(OnCloseWindow);
            TogglePluginConfigurationSignal.AddListener(OnTogglePluginConfigurationView);

            View.Visible = false; // plugin configuration views not visible on start

            View.PluginInfo = PluginInfo;
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.CloseWindow.RemoveListener(OnCloseWindow);
            CloseAllWindowsSignal.RemoveListener(OnCloseWindow);
            TogglePluginConfigurationSignal.RemoveListener(OnTogglePluginConfigurationView);
        }


        private void OnCloseWindow()
        {
            View.Visible = false;
        }


        private void OnTogglePluginConfigurationView(IPluginInfo targetPlugin)
        {
            if (!ReferenceEquals(PluginInfo, targetPlugin))
                return;

            View.Visible = !View.Visible;
        }
    }
}
