extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Config.Keys;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Serialization;
using strange.extensions.injector;
using strange.extensions.signal.impl;
using UnityEngine;
using ConfigNode = KSP::ConfigNode;
using HighLogic = KSP::HighLogic;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ConfigurationView : StrangeView
    {
        [Inject(StyleKey.TitleBarButtonStyle)] public GUIStyle TitleBarButtonStyle { get; set; }
        [Inject(TextureNameKey.CloseButton)] public Texture2D CloseButtonTexture { get; set; }
        [Inject(TextureNameKey.RescaleCursor)] public Texture2D RescaleCursorTexture { get; set; }

        [Inject] public GUISkin WindowSkin { get; set; }


        internal readonly Signal CloseWindow = new Signal();

        private WindowScale _scale;

        protected override IWindowComponent Initialize()
        {
            Skin = WindowSkin;
            Draggable = true;

            _scale = new WindowScale(this, Vector2.one);

            var adjustable = new AdjustableScale(_scale, ResizableHotzoneSize, MinWindowSize, 0.75f, 2f,
                RescaleCursorTexture, 0.1f, Vector2.one, AllowRescaling);



            var withButtons = new TitleBarButtons(adjustable, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(TitleBarButtonStyle, CloseButtonTexture,
                () => CloseWindow.Dispatch()));

            var clamp = new ClampToScreen(withButtons);

            return clamp;
        }


        private static bool AllowRescaling()
        {
            return Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt);
        }



        protected override void DrawWindow()
        {
            GUILayout.Label("This is the CoreConfiguration view");
            GUILayout.Label("Still under construction");

        }


        protected override void FinalizeWindow()
        {

        }


        public void SetScale(float f)
        {
            _scale.Scale = new Vector2(f, f);
        }
    }
}
