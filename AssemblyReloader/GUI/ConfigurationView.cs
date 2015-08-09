using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
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


        protected override IWindowComponent Initialize()
        {
            //Skin = HighLogic.Skin;
            Skin = WindowSkin;
            Draggable = true;

            var clamp = new ClampToScreen(this);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture,
                () => CloseWindow.Dispatch()));

            //var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, ResizeCursorTexture)
            //{
            //    Title = "Assembly Reloader"
            //};

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
