using System;
using System.Linq;
using AssemblyReloader.Config;
using AssemblyReloader.Logging;
using AssemblyReloader.PluginTracking;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Logic;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Buttons;
using ReeperCommon.Gui.Window.Decorators;
using ReeperCommon.Gui.Window.Providers;
using ReeperCommon.Gui.Window.View;
using ReeperCommon.Logging.Implementations;
using ReeperCommon.Repositories.Resources;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class WindowFactory
    {
        private readonly IResourceRepository _resourceProvider;
        private readonly GUISkin _windowSkin;

        public WindowFactory(
            IResourceRepository resourceProvider,
            
            GUISkin windowSkin)
        {
            if (resourceProvider == null) throw new ArgumentNullException("resourceProvider");
            if (windowSkin == null) throw new ArgumentNullException("windowSkin");

            _resourceProvider = resourceProvider;
            _windowSkin = windowSkin;
        }



        public void CreateMainWindow(
            View logic,
            Rect initialRect,
            int winid)
        {
            if (logic == null) throw new ArgumentNullException("logic");

            var basicWindow = new BasicWindow(logic, initialRect, winid, _windowSkin) { Title = "ART: Assembly Reloading Tool" };


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


        public IWindowComponent CreatePluginOptionsWindow(IReloadablePlugin plugin, IExpandablePanelFactory panelFactory)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (panelFactory == null) throw new ArgumentNullException("panelFactory");


            var basicWindow = new BasicWindow(new ConfigurationViewLogic(plugin.Configuration, panelFactory), new Rect(300f, 300f, 300f, 300f),
                UnityEngine.Random.Range(10000, 3434343), _windowSkin);

            basicWindow.Title = plugin.Name + " Configuration";

            //var decoratedWindow = new HideOnF2(new ClampToScreen(basicWindow)); buggy
            var decoratedWindow = new ClampToScreen(basicWindow);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decoratedWindow));

            return decoratedWindow;
        }


        public IExpandablePanelFactory CreateExpandablePanelFactory(GUISkin skin)
        {
            if (skin == null) throw new ArgumentNullException("skin");


            var style = new GUIStyle(skin.toggle);



            return new ExpandablePanelFactory(style, 12f, 0f, false);
        }
    }
}
