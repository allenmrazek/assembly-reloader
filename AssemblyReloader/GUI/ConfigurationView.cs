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
        internal readonly Signal CloseWindow = new Signal();


        protected override IWindowComponent Initialize()
        {
            Skin = HighLogic.Skin;
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
            GUILayout.Label("This is the Configuration view");
        }

        protected override void FinalizeWindow()
        {

        }
    }
}
