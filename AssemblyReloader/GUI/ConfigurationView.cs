using AssemblyReloader.Config.Keys;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Serialization;
using strange.extensions.injector;
using strange.extensions.signal.impl;
using UnityEngine;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ConfigurationView : StrangeView
    {
        [Inject(StyleKey.TitleBarButtonStyle)]
        public GUIStyle TitleBarButtonStyle { get; set; }
        [Inject(TextureNameKey.CloseButton)]
        public Texture2D CloseButtonTexture { get; set; }
        [Inject(TextureNameKey.SettingsButton)]
        public Texture2D SettingsButtonTexture { get; set; }
        [Inject(TextureNameKey.ResizeCursor)]
        public Texture2D ResizeCursorTexture { get; set; }
        [Inject]
        public GUISkin WindowSkin { get; set; }


        internal readonly Signal CloseWindow = new Signal();

        [ReeperPersistent] private string UnusedTestString = "Testvalue";

        protected override IWindowComponent Initialize()
        {
            Skin = WindowSkin;
            Draggable = true;

            var clamp = new ClampToScreen(this);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture,
                () => CloseWindow.Dispatch()));

            return withButtons;
        }



        protected override void DrawWindow()
        {
            GUILayout.Label("This is the CoreConfiguration view");
        }

        protected override void FinalizeWindow()
        {

        }
    }
}
