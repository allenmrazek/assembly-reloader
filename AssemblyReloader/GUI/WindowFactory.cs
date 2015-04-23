using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using ReeperCommon.Gui.Controls;
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


        public IWindowComponent CreatePluginOptionsWindow(IReloadablePlugin plugin, IExpandablePanelFactory panelFactory)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (panelFactory == null) throw new ArgumentNullException("panelFactory");

            var configLogic = new ConfigurationViewLogic(plugin.Configuration);

            foreach (var panel in CreateExpandingPanelsForConfiguration(configLogic, panelFactory, plugin.Configuration))
                configLogic.AddControl(panel);

            var basicWindow = new BasicWindow(configLogic, new Rect(300f, 300f, 300f, 300f),
                UnityEngine.Random.Range(10000, 3434343), _windowSkin, true);

            basicWindow.Title = plugin.Name + " Configuration";

            //var decoratedWindow = new HideOnF2(new ClampToScreen(basicWindow)); //buggy?
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


        private IEnumerable<ICustomControl> CreateExpandingPanelsForConfiguration(
            [NotNull] ConfigurationViewLogic view,
            [NotNull] IExpandablePanelFactory panelFactory,
            [NotNull] Configuration config)
        {
            if (view == null) throw new ArgumentNullException("view");
            if (panelFactory == null) throw new ArgumentNullException("panelFactory");
            if (config == null) throw new ArgumentNullException("config");

            return new[]
            {
                panelFactory.Create("General", view.DrawGeneralOptionsPanel),
                panelFactory.Create("Intermediate Language Weaving", view.DrawIntermediateLanguagePanel),
                panelFactory.Create("KSPAddon Options", view.DrawAddonPanel),
                panelFactory.Create("PartModule Options", view.DrawPartModulePanel),
                panelFactory.Create("ScenarioModule Options", view.DrawScenarioModulePanel)
            };
        }
    }
}
