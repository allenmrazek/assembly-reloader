extern alias KSP;
using System;
using ReeperCommon.Containers;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;
using ConfigNode = KSP::ConfigNode;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.ReloadablePlugin.Loaders.VesselModules;
using AssemblyReloader.ReloadablePlugin.Weaving;
using ReeperCommon.Logging;
using strange.extensions.injector;
using strange.extensions.mediation.impl;

namespace AssemblyReloader.ReloadablePlugin.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PluginConfigurationViewMediator : Mediator
    {
        private const string ConfigurationNodeName = "PluginConfigurationView";
        private PluginConfigurationView _view;

// ReSharper disable MemberCanBePrivate.Global
        [Inject] public PluginConfigurationView View {
            get { return _view; }
            set { _view = value; }
        }

        [Inject] public ILog Log { get; set; }
        [Inject] public IPluginInfo PluginInfo { get; set; }
        [Inject] public IAddonSettings AddonSettings { get; set; }
        [Inject] public IPartModuleSettings PartModuleSettings { get; set; }
        [Inject] public IScenarioModuleSettings ScenarioModuleSettings { get; set; }
        [Inject] public IVesselModuleSettings VesselModuleSettings { get; set; }
        [Inject] public IWeaverSettings WeaverSettings { get; set; }
        [Inject] public IConfigNodeSerializer Serializer { get; set; }

        [Inject] public PluginConfiguration Configuration { get; set; }

        [Inject] public SignalCloseAllWindows CloseAllWindowsSignal { get; set; }
        [Inject] public SignalTogglePluginConfigurationView TogglePluginConfigurationSignal { get; set; }

        [Inject] public SignalOnLoadConfiguration LoadConfigurationSignal { get; set; }
        [Inject] public SignalOnSaveConfiguration SaveConfigurationSignal { get; set; }

        [Inject] public SignalTriggerConfigurationSave TriggerSaveSignal { get; set; }


        public override void OnRegister()
        {
            Log.Debug("PluginConfigurationViewMediator starting");

            base.OnRegister();

            View.Visible = false; // plugin configuration views not visible on start

            View.PluginInfo = PluginInfo;
            View.AddonSettings = AddonSettings;
            View.PartModuleSettings = PartModuleSettings;
            View.ScenarioModuleSettings = ScenarioModuleSettings;
            View.VesselModuleSettings = VesselModuleSettings;
            View.WeaverSettings = WeaverSettings;
       

            View.CloseWindow.AddListener(OnCloseWindow);

            View.ToggleInstantAppliesToEveryScene.AddListener(OnToggleInstantlyAppliesToAllScenes);
            View.ToggleStartAddonsForCurrentScene.AddListener(OnToggleStartAddonsForCurrentScene);

            View.ToggleSaveAndReloadPartModuleConfigNodes.AddListener(OnToggleSaveAndReloadPartModuleConfigNodes);
            View.ToggleCreatePartModulesImmediately.AddListener(OnToggleCreatePartModulesImmediately);
            View.ToggleResetPartModuleActions.AddListener(OnToggleResetPartModuleActions);
            View.ToggleResetPartModuleEvents.AddListener(OnToggleToggleResetPartModuleEvents);

            View.ToggleCreateScenarioModulesImmediately.AddListener(OnToggleCreateScenarioModulesImmediately);
            View.ToggleSaveScenarioModulesBeforeDestruction.AddListener(OnToggleSaveScenarioModulesBeforeDestruction);

            View.ToggleCreateVesselModulesImmediately.AddListener(OnToggleCreateVesselModulesImmediately);

            View.ToggleWritePatchedAssemblyDataToDisk.AddListener(OnToggleWritePatchedAssemblyDataToDisk);
            View.ToggleInterceptGameEvents.AddListener(OnToggleInterceptGameEvents);
            View.ToggleDontInlineFunctionsThatCallGameEvents.AddListener(OnToggleDontInlineFunctionsThatCallGameEvents);

            CloseAllWindowsSignal.AddListener(OnCloseWindow);
            TogglePluginConfigurationSignal.AddListener(OnTogglePluginConfigurationView);

            LoadConfigurationSignal.AddListener(OnLoadConfiguration);
            SaveConfigurationSignal.AddListener(OnSaveConfiguration);
        }


        public override void OnRemove()
        {
            base.OnRemove();

            View.CloseWindow.RemoveListener(OnCloseWindow);

            View.ToggleInstantAppliesToEveryScene.RemoveListener(OnToggleInstantlyAppliesToAllScenes);
            View.ToggleStartAddonsForCurrentScene.RemoveListener(OnToggleStartAddonsForCurrentScene);

            View.ToggleSaveAndReloadPartModuleConfigNodes.RemoveListener(OnToggleSaveAndReloadPartModuleConfigNodes);
            View.ToggleCreatePartModulesImmediately.RemoveListener(OnToggleCreatePartModulesImmediately);
            View.ToggleResetPartModuleActions.RemoveListener(OnToggleResetPartModuleActions);
            View.ToggleResetPartModuleEvents.RemoveListener(OnToggleToggleResetPartModuleEvents);

            View.ToggleCreateScenarioModulesImmediately.RemoveListener(OnToggleCreateScenarioModulesImmediately);
            View.ToggleSaveScenarioModulesBeforeDestruction.RemoveListener(OnToggleSaveScenarioModulesBeforeDestruction);

            View.ToggleCreateVesselModulesImmediately.RemoveListener(OnToggleCreateVesselModulesImmediately);

            View.ToggleWritePatchedAssemblyDataToDisk.RemoveListener(OnToggleWritePatchedAssemblyDataToDisk);
            View.ToggleInterceptGameEvents.RemoveListener(OnToggleInterceptGameEvents);
            View.ToggleDontInlineFunctionsThatCallGameEvents.RemoveListener(OnToggleDontInlineFunctionsThatCallGameEvents);

            CloseAllWindowsSignal.RemoveListener(OnCloseWindow);
            TogglePluginConfigurationSignal.RemoveListener(OnTogglePluginConfigurationView);

            LoadConfigurationSignal.RemoveListener(OnLoadConfiguration);
            SaveConfigurationSignal.RemoveListener(OnSaveConfiguration);
        }


        private void OnCloseWindow()
        {
            View.Visible = false;
            TriggerSaveSignal.Dispatch();
        }


        private void OnTogglePluginConfigurationView(IPluginInfo targetPlugin)
        {
            if (!ReferenceEquals(PluginInfo, targetPlugin))
                return;

            View.Visible = !View.Visible;

            if (!View.Visible)
                TriggerSaveSignal.Dispatch();
        }


        private void OnLoadConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                config
                    .With(c => c.GetNode(ConfigurationNodeName))
                    .Do(n =>
                    {
                        Log.Verbose("Deserializing PluginConfigurationView");
                        Serializer.LoadObjectFromConfigNode(ref _view, n);
                        Log.Verbose("Successfully deserialized PluginConfigurationView");
                    }).IfNull(() => Log.Warning("No serialized settings found for PluginConfigurationView"));
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed to deserialize PluginConfigurationView: " + rse);
            }
        }

        private void OnSaveConfiguration(ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            try
            {
                Log.Verbose("Serializing PluginConfigurationView");
                Serializer.WriteObjectToConfigNode(ref _view, config.AddNode(ConfigurationNodeName));
                Log.Verbose("Successfully serialized PluginConfigurationView");
            }
            catch (ReeperSerializationException rse)
            {
                Log.Error("Failed to serialize PluginConfigurationView: " + rse);
            }
        }

        private void OnToggleInstantlyAppliesToAllScenes()
        {
            Configuration.InstantAppliesToEveryScene = !Configuration.InstantAppliesToEveryScene;
        }

        private void OnToggleStartAddonsForCurrentScene()
        {
            Configuration.StartAddonsForCurrentScene = !Configuration.StartAddonsForCurrentScene;
        }

        private void OnToggleSaveAndReloadPartModuleConfigNodes()
        {
            Configuration.SaveAndReloadPartModuleConfigNodes = !Configuration.SaveAndReloadPartModuleConfigNodes;
        }

        private void OnToggleCreatePartModulesImmediately()
        {
            Configuration.CreatePartModulesImmediately = !Configuration.CreatePartModulesImmediately;
        }

        private void OnToggleResetPartModuleActions()
        {
            Configuration.ResetPartModuleActions = !Configuration.ResetPartModuleActions;
        }

        private void OnToggleToggleResetPartModuleEvents()
        {
            Configuration.ResetPartModuleEvents = !Configuration.ResetPartModuleEvents;
        }

        private void OnToggleCreateScenarioModulesImmediately()
        {
            Configuration.CreateScenarioModulesImmediately = !Configuration.CreateScenarioModulesImmediately;
        }

        private void OnToggleSaveScenarioModulesBeforeDestruction()
        {
            Configuration.SaveScenarioModulesBeforeDestruction = !Configuration.SaveScenarioModulesBeforeDestruction;
        }

        private void OnToggleCreateVesselModulesImmediately()
        {
            Configuration.CreateVesselModulesImmediately = !Configuration.CreateVesselModulesImmediately;
        }


        private void OnToggleWritePatchedAssemblyDataToDisk()
        {
            Configuration.WritePatchedAssemblyDataToDisk = !Configuration.WritePatchedAssemblyDataToDisk;
        }

        private void OnToggleInterceptGameEvents()
        {
            Configuration.InterceptGameEvents = !Configuration.InterceptGameEvents;
        }

        private void OnToggleDontInlineFunctionsThatCallGameEvents()
        {
            Configuration.DontInlineFunctionsThatCallGameEvents = !Configuration.DontInlineFunctionsThatCallGameEvents;
        }
    }
}
