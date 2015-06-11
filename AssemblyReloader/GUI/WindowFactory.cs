using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.View;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class WindowFactory
    {
        private readonly IMessageChannel _viewMessageChannel;
        private readonly IWindowIdProvider _idProvider;
        private readonly IController _controller;
        private readonly GUIStyle _titleBarButtonStyle;
        private readonly Texture2D _optionsButtonTexture;
        private readonly Texture2D _closeButtonTexture;
        private readonly Vector2 _titleBarButtonOffset = new Vector2(3f, 3f);

        public WindowFactory(
            [NotNull] IMessageChannel viewMessageChannel, 
            [NotNull] IWindowIdProvider idProvider,
            [NotNull] IController controller, 
            [NotNull] GUIStyle titleBarButtonStyle, 
            [NotNull] Texture2D optionsButtonTexture,
            [NotNull] Texture2D closeButtonTexture)
        {
            if (viewMessageChannel == null) throw new ArgumentNullException("viewMessageChannel");
            if (idProvider == null) throw new ArgumentNullException("idProvider");
            if (controller == null) throw new ArgumentNullException("controller");
            if (titleBarButtonStyle == null) throw new ArgumentNullException("titleBarButtonStyle");
            if (optionsButtonTexture == null) throw new ArgumentNullException("optionsButtonTexture");
            if (closeButtonTexture == null) throw new ArgumentNullException("closeButtonTexture");

            _viewMessageChannel = viewMessageChannel;
            _idProvider = idProvider;
            _controller = controller;
            _titleBarButtonStyle = titleBarButtonStyle;
            _optionsButtonTexture = optionsButtonTexture;
            _closeButtonTexture = closeButtonTexture;
        }


        public IWindowComponent CreateMainWindow(
            [NotNull] IEnumerable<IPluginInfo> plugins, 
            WindowAppearanceInfo appearanceInfo, 
            Maybe<ConfigNode> windowConfig)
        {
            if (plugins == null) throw new ArgumentNullException("plugins");

            var mainWindow = new MainWindow(plugins, _viewMessageChannel, appearanceInfo.InitialSize, _idProvider.Get(), appearanceInfo.Skin,
                true) { Title = "Assembly Reload Tool" };

            var resizable = new Resizable(mainWindow, appearanceInfo.DragHotzoneSize, appearanceInfo.MinDimensions,
                appearanceInfo.DragCursorTexture);

            var clamp = new ClampToScreen(resizable);
            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, _titleBarButtonOffset);

            withButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, _optionsButtonTexture,
                s => mainWindow.OnOptionsButton(), "Options"));

            withButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
                s => mainWindow.OnCloseButton(), "Close"));



            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(withButtons, "MainWindow"));

            if (windowConfig.Any())
                withButtons.Load(windowConfig.Single());

            return withButtons;
        }
    }
}
