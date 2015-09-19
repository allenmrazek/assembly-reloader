extern alias KSP;
using System;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.ReloadablePlugin.Loaders.VesselModules;
using AssemblyReloader.ReloadablePlugin.Weaving;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using strange.extensions.injector;
using strange.extensions.signal.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Gui
{
    public class PluginConfigurationView : StrangeView
    {
        [Inject(StyleKey.TitleBarButtonStyle)] public GUIStyle TitleBarButtonStyle { get; set; }
        [Inject(TextureNameKey.CloseButton)] public Texture2D CloseButtonTexture { get; set; }
        [Inject(TextureNameKey.SettingsButton)] public Texture2D SettingsButtonTexture { get; set; }
        [Inject(TextureNameKey.ResizeCursor)] public Texture2D ResizeCursorTexture { get; set; }
        [Inject] public GUISkin WindowSkin { get; set; }


        public IPluginInfo PluginInfo { get; set; }
        public IAddonSettings AddonSettings { get; set; }
        public IPartModuleSettings PartModuleSettings { get; set; }
        public IScenarioModuleSettings ScenarioModuleSettings { get; set; }
        public IVesselModuleSettings VesselModuleSettings { get; set; }
        public IWeaverSettings WeaverSettings { get; set; }


        internal readonly Signal CloseWindow = new Signal();
        private Vector2 _scroll = default(Vector2);

        // AddonSettings
        internal readonly Signal ToggleInstantAppliesToEveryScene = new Signal();
        internal readonly Signal ToggleStartAddonsForCurrentScene = new Signal();

        // PartModuleSettings
        internal readonly Signal ToggleSaveAndReloadPartModuleConfigNodes = new Signal();
        internal readonly Signal ToggleCreatePartModulesImmediately = new Signal();
        internal readonly Signal ToggleResetPartModuleActions = new Signal();
        internal readonly Signal ToggleResetPartModuleEvents = new Signal();

        // ScenarioModuleSettings
        internal readonly Signal ToggleCreateScenarioModulesImmediately = new Signal();
        internal readonly Signal ToggleSaveScenarioModulesBeforeDestruction = new Signal();

        // VesselModuleSettings
        internal readonly Signal ToggleCreateVesselModulesImmediately = new Signal();

        // WeaverSettings
        internal readonly Signal ToggleWritePatchedAssemblyDataToDisk = new Signal();
        internal readonly Signal ToggleInterceptGameEvents = new Signal();
        internal readonly Signal ToggleDontInlineFunctionsThatCallGameEvents = new Signal();


        protected override IWindowComponent Initialize()
        {
            Skin = WindowSkin;
            Draggable = true;

            var clamp = new ClampToScreen(this);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture,
                () => CloseWindow.Dispatch()));

            var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, ResizeCursorTexture)
            {
                Title = PluginInfo.Name + " Configuration",
                Dimensions = new Rect(0f, 0f, 300f, 300f)
            };

            return resizable;
        }


        protected override void DrawWindow()
        {
            _scroll = GUILayout.BeginScrollView(_scroll);
            {
                DrawSection("Addon Settings", DrawAddonSettings);
                DrawSection("PartModule Settings", DrawPartModuleSettings);
                DrawSection("ScenarioModule Settings", DrawScenarioModuleSettings);
                DrawSection("VesselModule Settings", DrawVesselModuleSettings);
                DrawSection("Weaver Settings", DrawWeaverSettings);
            }
            GUILayout.EndScrollView();
        }


        private static void DrawSection(string heading, Action drawCallback)
        {
            GUILayout.Label(heading);
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10f);
                GUILayout.BeginVertical();
                {
                    drawCallback();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }


        private void DrawAddonSettings()
        {
            DrawToggleSetting("KSPAddon.Startup.Instant applies to every scene", AddonSettings.InstantAppliesToEveryScene,
                ToggleInstantAppliesToEveryScene);

            DrawToggleSetting("Start KSPAddons for current scene", AddonSettings.StartAddonsForCurrentScene,
                ToggleStartAddonsForCurrentScene);
        }


        private void DrawPartModuleSettings()
        {
            DrawToggleSetting("Save and reload PartModule ConfigNodes", PartModuleSettings.SaveAndReloadPartModuleConfigNodes,
                ToggleSaveAndReloadPartModuleConfigNodes);

            DrawToggleSetting("Reload PartModule instances immediately", PartModuleSettings.CreatePartModulesImmediately,
                ToggleCreatePartModulesImmediately);

            DrawToggleSetting("Reset PartModule Actions", PartModuleSettings.ResetPartModuleActions, ToggleResetPartModuleActions);

            DrawToggleSetting("Reset PartModule Events", PartModuleSettings.ResetPartModuleEvents,
                ToggleResetPartModuleEvents);
        }


        private void DrawScenarioModuleSettings()
        {
            DrawToggleSetting("Reload ScenarioModules immediately", ScenarioModuleSettings.CreateScenarioModulesImmediately,
                ToggleCreateScenarioModulesImmediately);

            DrawToggleSetting("Save ScenarioModules before destruction", ScenarioModuleSettings.SaveScenarioModulesBeforeDestruction,
                ToggleSaveScenarioModulesBeforeDestruction);
        }


        private void DrawVesselModuleSettings()
        {
            DrawToggleSetting("Create VesselModule instances immediately",
                VesselModuleSettings.CreateVesselModulesImmediately, ToggleCreateVesselModulesImmediately);
        }


        private void DrawWeaverSettings()
        {
            DrawToggleSetting("Write patched assembly to disk", WeaverSettings.WritePatchedAssemblyDataToDisk,
                ToggleWritePatchedAssemblyDataToDisk);

            DrawToggleSetting("Intercept GameEvents", WeaverSettings.InterceptGameEvents, ToggleInterceptGameEvents);

            if (!WeaverSettings.InterceptGameEvents)
                GUI.enabled = false;
            DrawToggleSetting("Don't inline methods that call GameEvents", WeaverSettings.DontInlineFunctionsThatCallGameEvents,
                ToggleDontInlineFunctionsThatCallGameEvents);
            GUI.enabled = true;
        }


        protected override void FinalizeWindow()
        {
            // no-op
        }


        private static void DrawToggleSetting(string text, bool value, Signal signal)
        {
            var result = GUILayout.Toggle(value, text, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            if (result != value)
                signal.Dispatch();
        }
    }
}
