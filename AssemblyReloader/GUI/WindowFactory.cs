using System;
using AssemblyReloader.Config.Names;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.View;
using UnityEngine;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class WindowFactory
    {
        private static readonly Vector2 TitleBarButtonOffset = new Vector2(2f, 2f);
        private static readonly Vector2 ResizableHotzoneSize = new Vector2(10f, 10f);
        private static readonly Vector2 MinWindowSize = new Vector2(200f, 100f);

        private static GUIStyle _titleBarButtonStyle = new GUIStyle();
        private static Texture2D _closeButtonTexture = new Texture2D(1, 1);
        private static Texture2D _settingsButtonTexture = new Texture2D(1, 1);
        private static Texture2D _resizeCursorTexture = new Texture2D(1, 1);


        //public WindowFactory(
        //    [NotNull, Name(Styles.TitleBarButtonStyle)] GUIStyle titleBarButtonStyle,
        //    [NotNull, Name(TextureNames.CloseButton)] Texture2D closeButtonTexture,
        //    [NotNull, Name(TextureNames.SettingsButton)] Texture2D settingsButtonTexture,
        //    [NotNull, Name(TextureNames.ResizeCursor)] Texture2D resizeCursorTexture)
        //{
        //    if (titleBarButtonStyle == null) throw new ArgumentNullException("titleBarButtonStyle");
        //    if (closeButtonTexture == null) throw new ArgumentNullException("closeButtonTexture");
        //    if (settingsButtonTexture == null) throw new ArgumentNullException("settingsButtonTexture");
        //    if (resizeCursorTexture == null) throw new ArgumentNullException("resizeCursorTexture");

        //    _titleBarButtonStyle = titleBarButtonStyle;
        //    _closeButtonTexture = closeButtonTexture;
        //    _settingsButtonTexture = settingsButtonTexture;
        //    _resizeCursorTexture = resizeCursorTexture;
        //}

        public static void Initialize(
            [NotNull, Name(Styles.TitleBarButtonStyle)] GUIStyle titleBarButtonStyle,
            [NotNull, Name(TextureNames.CloseButton)] Texture2D closeButtonTexture,
            [NotNull, Name(TextureNames.SettingsButton)] Texture2D settingsButtonTexture,
            [NotNull, Name(TextureNames.ResizeCursor)] Texture2D resizeCursorTexture)
        {
            _titleBarButtonStyle = titleBarButtonStyle;
            _closeButtonTexture = closeButtonTexture;
            _settingsButtonTexture = settingsButtonTexture;
            _resizeCursorTexture = resizeCursorTexture;
        }




        //public WindowView CreateMainWindow(
        //    [NotNull] IMainView mainWindowLogic, 
        //    [NotNull] IViewMediator mediator)
        //{
        //    if (mainWindowLogic == null) throw new ArgumentNullException("mainWindowLogic");
        //    if (mediator == null) throw new ArgumentNullException("mediator");

        //    var clamp = new ClampToScreen(mainWindowLogic);

        //    var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

        //    withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _settingsButtonTexture,
        //        mainWindowLogic.ToggleSettings));

        //    withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
        //        mainWindowLogic.Close));

        //    var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, _resizeCursorTexture)
        //    {
        //        Title = "Assembly Reloader"
        //    };

        //    var view = WindowView.Create(resizable, "MainWindow");
        //    UnityEngine.Object.DontDestroyOnLoad(view);

        //    return view;
        //}


        public static IWindowComponent CreateMainWindow(MainView view)
        {
            var baseLogic = new MainWindowLogic();
            var clamp = new ClampToScreen(baseLogic);

            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _settingsButtonTexture,
                baseLogic.ToggleSettings));

            withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
                baseLogic.Close));

            var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, _resizeCursorTexture)
            {
                Title = "Assembly Reloader"
            };

            return resizable;
        }


        //public WindowView CreateSettingsWindow([NotNull] ISettingsView settingsLogic, [NotNull] IViewMediator mediator)
        //{
        //    if (settingsLogic == null) throw new ArgumentNullException("settingsLogic");
        //    if (mediator == null) throw new ArgumentNullException("mediator");

        //    var clamp = new ClampToScreen(settingsLogic);

        //    var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

        //    withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
        //        settingsLogic.Close));

        //    var resizable = new Resizable(withButtons, ResizableHotzoneSize, MinWindowSize, _resizeCursorTexture)
        //    {
        //        Title = "General Settings"
        //    };

        //    var view = WindowView.Create(resizable, "SettingsWindow");
        //    UnityEngine.Object.DontDestroyOnLoad(view);

        //    resizable.Visible = false;
        //    return view;
        //}
    }
}
