﻿using System;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Logic;
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
            IWindowLogic programConfigurationLogic,
            Rect initialRect,
            int winid)
        {
            if (logic == null) throw new ArgumentNullException("logic");

            var basicWindow = new BasicWindow(logic, initialRect, winid, _windowSkin) { Title = "Assembly Reload Tool" };
            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, GetTexture("Resources/btnWrench.png"), s => { }, "Test2"));
            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, GetTexture("Resources/btnClose.png"), s => { }, "Test"));
            
            var hiding = new HideOnF2(tbButtons);
            var clamp = new ClampToScreen(hiding);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(clamp, "MainWindow"));
        }


       
        public IWindowComponent CreatePluginOptionsWindow(
            [NotNull] PluginConfigurationViewLogic pluginViewLogic,
            [NotNull] IReloadablePlugin plugin,
            [NotNull] ICommand saveOptionsCommand,
            int winid)
        {
            if (pluginViewLogic == null) throw new ArgumentNullException("pluginViewLogic");
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (saveOptionsCommand == null) throw new ArgumentNullException("saveOptionsCommand");


            foreach (var panel in _configurationPanelFactory.CreatePanelsFor(plugin.Configuration))
                pluginViewLogic.AddPanel(panel);

            var basicWindow = new BasicWindow(pluginViewLogic, new Rect(300f, 300f, 450f, 300f),
                winid, _windowSkin) {Title = plugin.Name + " Configuration"};


            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle,
                GetTexture("Resources/btnClose.png"),
                s =>
                {
                    tbButtons.Visible = false;
                    saveOptionsCommand.Execute();
                }, "Close"));
            
            //var decoratedWindow = new HideOnF2(new ClampToScreen(basicWindow)); //buggy?
            var decoratedWindow = new ClampToScreen(tbButtons);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decoratedWindow));

            return decoratedWindow;
        }


        public IWindowComponent CreateProgramOptionsWindow([NotNull] ProgramConfigurationViewLogic logic,
            [NotNull] Configuration programConfiguration, 
            [NotNull] ICommand saveOptionsCommand,
            int winid)
        {
            if (logic == null) throw new ArgumentNullException("logic");
            if (programConfiguration == null) throw new ArgumentNullException("programConfiguration");
            if (saveOptionsCommand == null) throw new ArgumentNullException("saveOptionsCommand");


            var basicWindow = new BasicWindow(logic, new Rect(450f, 300f, 300f, 200f), winid, _windowSkin)
            {
                Title = "Program Configuration"
            };

            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, GetTexture("Resources/btnClose.png"),
                s =>
                {
                    tbButtons.Visible = false;
                    saveOptionsCommand.Execute();
                }, "Close"));

            var decorated = new ClampToScreen(tbButtons);

            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decorated));

            return decorated;
        }


        Texture2D GetTexture(string url)
        {
            var tex = _resourceProvider.GetTexture(url);

            if (!tex.Any())
                throw new Exception("Could not find texture at " + url);

            return tex.Single();
        }
    }
}
