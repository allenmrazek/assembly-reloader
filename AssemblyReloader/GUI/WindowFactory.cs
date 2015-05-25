using System;
using System.Linq;
using System.Runtime.InteropServices;
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

        private const string CloseButtonUrl = "Resources/btnClose.png";
        private const string OptionsButtonUrl = "Resources/btnWrench.png";

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
            [NotNull] IWindowComponent programConfigurationWindow, 
            [NotNull] ICommand saveProgramConfiguration,
            Rect initialRect,
            int winid)
        {
            if (logic == null) throw new ArgumentNullException("logic");
            if (programConfigurationWindow == null) throw new ArgumentNullException("programConfigurationWindow");
            if (saveProgramConfiguration == null) throw new ArgumentNullException("saveProgramConfiguration");

            var basicWindow = new BasicWindow(logic, initialRect, winid, _windowSkin) { Title = "Assembly Reload Tool" };
            var tbButtons = new TitleBarButtons(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, GetTexture(OptionsButtonUrl), s =>
            {
                programConfigurationWindow.Visible = !programConfigurationWindow.Visible;
                saveProgramConfiguration.Execute();
            }, "ProgramConfigurationButton"));

            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, GetTexture(CloseButtonUrl), s => { }, "Test"));
            
            var hiding = new HideOnF2(tbButtons);
            var clamp = new ClampToScreen(hiding);

            programConfigurationWindow.Visible = false;

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

            var basicWindow = CreateBasicWindow(pluginViewLogic, new Rect(300f, 300f, 450f, 300f),
                winid, plugin.Name + " Configuration");


            var tbButtons = CreateButtonToolbar(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

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


            var basicWindow = CreateBasicWindow(logic, new Rect(450f, 300f, 300f, 200f), winid,
                "Program Configuration");

            var tbButtons = CreateButtonToolbar(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

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

        private IWindowComponent CreateBasicWindow(IWindowLogic logic, Rect rect, int winid, string title)
        {
            return new BasicWindow(logic, rect, winid, _windowSkin) { Title = title};
        }

        private TitleBarButtons CreateButtonToolbar(IWindowComponent window,
            TitleBarButtons.ButtonAlignment alignment, Vector2 offset)
        {
            return new TitleBarButtons(window, alignment, offset);
        }
    }
}
