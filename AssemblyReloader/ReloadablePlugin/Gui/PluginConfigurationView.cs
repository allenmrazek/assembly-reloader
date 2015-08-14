using AssemblyReloader.Config.Keys;
using AssemblyReloader.Gui;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
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


        internal readonly Signal CloseWindow = new Signal();
        internal readonly Signal ToggleInstantlyAppliesToAllScenesSignal = new Signal();



        protected override IWindowComponent Initialize()
        {
            //Skin = HighLogic.Skin;
            //Skin = UnityEngine.Object.Instantiate(AssetBase.GetGUISkin("OrbitMapSkin")) as GUISkin;
            Skin = WindowSkin;
            Draggable = true;

            var clamp = new ClampToScreen(this);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture,
                () => CloseWindow.Dispatch()));

            var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, ResizeCursorTexture)
            {
                Title = PluginInfo.Name + " Configuration"
            };

            return resizable;
        }


        protected override void DrawWindow()
        {
            GUILayout.Label("Addon Settings");

            GUILayout.BeginHorizontal();
            GUILayout.Space(10f);
            GUILayout.BeginVertical();
            DrawToggleSetting("Instantly applies to all scenes", AddonSettings.InstantlyAppliesToAllScenes,
                ToggleInstantlyAppliesToAllScenesSignal);
            GUILayout.Label("Addon setting 2");
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
        }


        protected override void FinalizeWindow()
        {
            // no-op
        }


        private void DrawToggleSetting(string text, bool value, Signal signal)
        {
            var result = GUILayout.Toggle(value, text, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            if (result != value)
                signal.Dispatch();
        }
    }
}
