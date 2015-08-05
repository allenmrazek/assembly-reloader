using AssemblyReloader.Gui;
using AssemblyReloader.StrangeIoC.extensions.signal.impl;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Gui
{
    public class PluginConfigurationView : StrangeView
    {
        public IPluginInfo PluginInfo { get; set; }


        internal readonly Signal CloseWindow = new Signal();

        protected override void Awake()
        {
            print("PluginConfigurationView.awake");

            base.Awake();
        }


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
            GUILayout.Label("Plugin Options for " + PluginInfo.Name + " go here");
        }


        protected override void FinalizeWindow()
        {
            // no-op
        }
    }
}
