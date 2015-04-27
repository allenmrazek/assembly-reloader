using System;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Controllers;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging;
using ReeperCommon.Repositories;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    class WindowFactory
    {
        private readonly IResourceRepository _resourceProvider;
        private readonly IConfigurationPanelFactory _configurationPanelFactory;
        private readonly GUISkin _windowSkin;
        private readonly GUIStyle _titleBarButtonStyle;
// ReSharper disable once InconsistentNaming
        private readonly Vector2 TitleBarButtonOffset = new Vector2(3f, 3f);

        public WindowFactory(
            [NotNull] IResourceRepository resourceProvider,
            [NotNull] IConfigurationPanelFactory configurationPanelFactory,
            [NotNull] GUISkin windowSkin, 
            [NotNull] GUIStyle titleBarButtonStyle)
        {
            if (resourceProvider == null) throw new ArgumentNullException("resourceProvider");
            if (configurationPanelFactory == null) throw new ArgumentNullException("configurationPanelFactory");
            if (windowSkin == null) throw new ArgumentNullException("windowSkin");
            if (titleBarButtonStyle == null) throw new ArgumentNullException("titleBarButtonStyle");

            _resourceProvider = resourceProvider;
            _configurationPanelFactory = configurationPanelFactory;
            _windowSkin = windowSkin;
            _titleBarButtonStyle = titleBarButtonStyle;
        }



        public void CreateMainWindow(
            [NotNull] View logic,
            Rect initialRect,
            int winid)
        {
            if (logic == null) throw new ArgumentNullException("logic");

            var basicWindow = new BasicWindow(logic, initialRect, winid, _windowSkin) { Title = "ART: Assembly Reloading Tool" };


            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);



            //var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            //style.fixedHeight = style.fixedWidth = 16f;
            //style.margin = new RectOffset();


            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, GetCloseButtonTexture(), s => { }, "Test"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test2"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test3"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdst2"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdfdst2"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefddsffsdst2"));

            var hiding = new HideOnF2(tbButtons);

            var clamp = new ClampToScreen(hiding);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(clamp, "MainWindow"));
        }


       
        public IWindowComponent CreatePluginOptionsWindow(
            [NotNull] IReloadablePlugin plugin,
            [NotNull] ICommand saveOptionsCommand)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (saveOptionsCommand == null) throw new ArgumentNullException("saveOptionsCommand");

            var configLogic = new ConfigurationViewLogic(plugin.Configuration);

            foreach (var panel in _configurationPanelFactory.CreatePanelsFor(plugin.Configuration)) 
                configLogic.AddPanel(panel);

            var basicWindow = new BasicWindow(configLogic, new Rect(300f, 300f, 450f, 300f),
                UnityEngine.Random.Range(10000, 3434343), _windowSkin);

            basicWindow.Title = plugin.Name + " Configuration";


            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle,
                GetCloseButtonTexture(),
                s =>
                {
                    new DebugLog().Normal("Button close!");
                    tbButtons.Visible = false;
                    saveOptionsCommand.Execute();
                }, "Close"));
            
            //var decoratedWindow = new HideOnF2(new ClampToScreen(basicWindow)); //buggy?
            var decoratedWindow = new ClampToScreen(tbButtons);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decoratedWindow));

            return decoratedWindow;
        }


        Texture2D GetCloseButtonTexture()
        {
            var tex = _resourceProvider.GetTexture("Resources/btnClose.png");

            if (!tex.Any())
                throw new Exception("Could not find close button texture");

            return tex.Single();
        }
    }
}
