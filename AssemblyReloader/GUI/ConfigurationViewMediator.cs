using AssemblyReloader.Config;
using ReeperCommon.Logging;
using strange.extensions.injector;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
    public class ConfigurationViewMediator : Mediator
    {
        [Inject] public SignalCloseAllWindows CloseAllWindows { get; set; }
        [Inject] public SignalToggleConfigurationView ToggleConfigurationViewSignal { get; set; }

        [Inject] public ConfigurationView View { get; set; }
        [Inject] public CoreConfiguration CoreConfiguration { get; set; }
        [Inject] public ILog Log { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            View.CloseWindow.AddListener(CloseConfigurationWindow);
            CloseAllWindows.AddListener(CloseConfigurationWindow);
            ToggleConfigurationViewSignal.AddListener(OnToggleConfigurationWindow);

            View.Visible = false; // initially start closed
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.CloseWindow.RemoveListener(CloseConfigurationWindow);
            CloseAllWindows.RemoveListener(CloseConfigurationWindow);
            ToggleConfigurationViewSignal.RemoveListener(OnToggleConfigurationWindow);
        }


        private void CloseConfigurationWindow()
        {
            Log.Debug("Received close configuration window");

            View.Visible = false;
        }


        private void OnToggleConfigurationWindow()
        {
            View.Visible = !View.Visible;
        }
    }
}
