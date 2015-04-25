using System;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Repositories.Resources;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    class WindowFactory
    {
        private readonly IResourceRepository _resourceProvider;
        private readonly IConfigurationPanelFactory _configurationPanelFactory;
        private readonly GUISkin _windowSkin;


        public WindowFactory(
            [NotNull] IResourceRepository resourceProvider,
            [NotNull] IConfigurationPanelFactory configurationPanelFactory,
            [NotNull] GUISkin windowSkin)
        {
            if (resourceProvider == null) throw new ArgumentNullException("resourceProvider");
            if (configurationPanelFactory == null) throw new ArgumentNullException("configurationPanelFactory");
            if (windowSkin == null) throw new ArgumentNullException("windowSkin");

            _resourceProvider = resourceProvider;
            _configurationPanelFactory = configurationPanelFactory;
            _windowSkin = windowSkin;
        }



        public void CreateMainWindow(
            [NotNull] View logic,
            Rect initialRect,
            int winid)
        {
            if (logic == null) throw new ArgumentNullException("logic");

            var basicWindow = new BasicWindow(logic, initialRect, winid, _windowSkin, true) { Title = "ART: Assembly Reloading Tool" };


            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, new Vector2(3f, 3f));



            var style = new GUIStyle(HighLogic.Skin.button) { border = new RectOffset(), padding = new RectOffset() };
            style.fixedHeight = style.fixedWidth = 16f;
            style.margin = new RectOffset();

            var btnClose = _resourceProvider.GetTexture("Resources/btnClose.png");

            tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test2"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Test3"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdst2"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefsdfdst2"));
            //tbButtons.AddButton(new TitleBarButton(style, btnClose.First(), s => { }, "Tefddsffsdst2"));

            var hiding = new HideOnF2(tbButtons);

            var clamp = new ClampToScreen(hiding);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(clamp, "MainWindow"));
        }


        public IWindowComponent CreatePluginOptionsWindow(IReloadablePlugin plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            var configLogic = new ConfigurationViewLogic(plugin.Configuration);

            foreach (var panel in _configurationPanelFactory.CreatePanelsFor(plugin.Configuration)) 
                configLogic.AddPanel(panel);

            var basicWindow = new BasicWindow(configLogic, new Rect(300f, 300f, 450f, 300f),
                UnityEngine.Random.Range(10000, 3434343), _windowSkin, true);

            basicWindow.Title = plugin.Name + " Configuration";

            //var decoratedWindow = new HideOnF2(new ClampToScreen(basicWindow)); //buggy?
            var decoratedWindow = new ClampToScreen(basicWindow);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decoratedWindow));

            return decoratedWindow;
        }
    }
}
