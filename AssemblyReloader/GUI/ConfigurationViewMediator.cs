using AssemblyReloader.Config;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.Gui
{
    class ConfigurationViewMediator : Mediator
    {
        [Inject] public ConfigurationView View { get; set; }

        [Inject] public Configuration Configuration { get; set; }

        [Inject]
        public SignalCloseAllWindows CloseAllWindows { get; set; }

        [Inject] public ILog Log { get; set; }

        public override void OnRegister()
        {
            base.OnRegister();

            View.CloseWindow.AddListener(CloseConfigurationWindow);
            CloseAllWindows.AddListener(CloseConfigurationWindow);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.CloseWindow.RemoveListener(CloseConfigurationWindow);
            CloseAllWindows.RemoveListener(CloseConfigurationWindow);
        }


        private void CloseConfigurationWindow()
        {
            Log.Debug("Received close configuration window");

            View.Visible = false;
        }
    }
}
