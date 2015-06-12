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
        private readonly IWindowIdProvider _idProvider;
        private readonly IController _controller;
        private readonly IMessageChannel _viewMessageChannel;
        private readonly GUIStyle _titleBarButtonStyle;
        private readonly Texture2D _optionsButtonTexture;
        private readonly Texture2D _closeButtonTexture;
        private readonly Vector2 _titleBarButtonOffset = new Vector2(3f, 3f);

        public WindowFactory(
            [NotNull] IWindowIdProvider idProvider,
            [NotNull] IController controller, 
            [NotNull] IMessageChannel viewMessageChannel,
            [NotNull] GUIStyle titleBarButtonStyle, 
            [NotNull] Texture2D optionsButtonTexture,
            [NotNull] Texture2D closeButtonTexture)
        {
            if (idProvider == null) throw new ArgumentNullException("idProvider");
            if (controller == null) throw new ArgumentNullException("controller");
            if (viewMessageChannel == null) throw new ArgumentNullException("viewMessageChannel");
            if (titleBarButtonStyle == null) throw new ArgumentNullException("titleBarButtonStyle");
            if (optionsButtonTexture == null) throw new ArgumentNullException("optionsButtonTexture");
            if (closeButtonTexture == null) throw new ArgumentNullException("closeButtonTexture");

            _idProvider = idProvider;
            _controller = controller;
            _viewMessageChannel = viewMessageChannel;
            _titleBarButtonStyle = titleBarButtonStyle;
            _optionsButtonTexture = optionsButtonTexture;
            _closeButtonTexture = closeButtonTexture;
        }


        public WindowDescriptor CreateMainWindow(
            [NotNull] IEnumerable<IPluginInfo> plugins,
            [NotNull] WindowAppearanceInfo appearanceInfo,
            Maybe<ConfigNode> windowConfig)
        {
            if (plugins == null) throw new ArgumentNullException("plugins");
            if (appearanceInfo == null) throw new ArgumentNullException("appearanceInfo");

            var mainWindow = new MainWindow(_controller, plugins, _viewMessageChannel, appearanceInfo.InitialSize, _idProvider.Get(), appearanceInfo.Skin,
                true) { Title = "Assembly Reload Tool" };

            var resizable = new Resizable(mainWindow, appearanceInfo.DragHotzoneSize, appearanceInfo.MinDimensions,
                appearanceInfo.DragCursorTexture);

            var clamp = new ClampToScreen(resizable);
            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, _titleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _optionsButtonTexture,
                mainWindow.OnOptionsButton));

            withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
                mainWindow.OnCloseButton));

            if (windowConfig.Any())
                withButtons.Load(windowConfig.Single());

            var view = WindowView.Create(withButtons, "MainWindow");
            UnityEngine.Object.DontDestroyOnLoad(view);

            return new WindowDescriptor(mainWindow, view);
        }


        public WindowDescriptor CreateOptionsWindow(
            [NotNull] WindowAppearanceInfo appearanceInfo,
            [NotNull] Configuration configuration,
            Maybe<ConfigNode> windowConfig)
        {
            if (appearanceInfo == null) throw new ArgumentNullException("appearanceInfo");
            if (configuration == null) throw new ArgumentNullException("configuration");

            var optionsWindow = new ConfigurationWindow(configuration, _controller, appearanceInfo.InitialSize,
                _idProvider.Get(), appearanceInfo.Skin) { Title = "Configuration" };

            var clamp = new ClampToScreen(optionsWindow);
            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, _titleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
                optionsWindow.OnCloseButton));

            if (windowConfig.Any())
                withButtons.Load(windowConfig.Single());

            withButtons.Visible = false;

            var view = WindowView.Create(withButtons, "ConfigurationWindow");
            UnityEngine.Object.DontDestroyOnLoad(view);


            return new WindowDescriptor(optionsWindow, view);
        }


        public WindowDescriptor CreatePluginOptionsWindow(
            [NotNull] WindowAppearanceInfo appearanceInfo,
            [NotNull] IPluginInfo plugin,
            Maybe<ConfigNode> windowConfig)
        {
            if (appearanceInfo == null) throw new ArgumentNullException("appearanceInfo");
            if (plugin == null) throw new ArgumentNullException("plugin");

            var pluginWindow = new PluginConfigurationWindow(_controller, plugin, appearanceInfo.InitialSize,
                _idProvider.Get(), appearanceInfo.Skin) { Title = plugin.Name + " Configuration"};

            var clamp = new ClampToScreen(pluginWindow);
            var withButtons = new TitleBarButtons(clamp, TitleBarButtons.ButtonAlignment.Right, _titleBarButtonOffset);

            withButtons.AddButton(new BasicTitleBarButton(_titleBarButtonStyle, _closeButtonTexture,
                pluginWindow.OnCloseButton));

            if (windowConfig.Any())
                withButtons.Load(windowConfig.Single());

            withButtons.Visible = false;

            var view = WindowView.Create(withButtons, "PluginConfigurationWindow_" + plugin.Name);
            UnityEngine.Object.DontDestroyOnLoad(view);


            return new WindowDescriptor(pluginWindow, view);
        }
    }
}
