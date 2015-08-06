using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
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
        [Inject] public IPluginInfo PluginInfo { get; set; }
        [Inject] public IAddonSettings AddonSettings { get; set; }
        [Inject] public IPartModuleSettings PartModuleSettings { get; set; }

        [Inject] public PluginConfiguration Configuration { get; set; }

        [Inject] public SignalCloseAllWindows CloseAllWindowsSignal { get; set; }
        [Inject] public SignalTogglePluginConfigurationView TogglePluginConfigurationSignal { get; set; }


        public override void OnRegister()
        {
            Log.Debug("PluginConfigurationViewMediator starting");

            base.OnRegister();

            View.Visible = false; // plugin configuration views not visible on start

            View.PluginInfo = PluginInfo;
            View.AddonSettings = AddonSettings;
            View.PartModuleSettings = PartModuleSettings;


            View.CloseWindow.AddListener(OnCloseWindow);
            View.ToggleInstantlyAppliesToAllScenesSignal.AddListener(OnToggleInstantlyAppliesToAllScenes);

            CloseAllWindowsSignal.AddListener(OnCloseWindow);
            TogglePluginConfigurationSignal.AddListener(OnTogglePluginConfigurationView);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.CloseWindow.RemoveListener(OnCloseWindow);
            View.ToggleInstantlyAppliesToAllScenesSignal.RemoveListener(OnToggleInstantlyAppliesToAllScenes);

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


        private void OnToggleInstantlyAppliesToAllScenes()
        {
            Configuration.InstantlyAppliesToAllScenes = !Configuration.InstantlyAppliesToAllScenes;
        }
    }
}
