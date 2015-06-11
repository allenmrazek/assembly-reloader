//using System;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Commands;
//using AssemblyReloader.CompositeRoot;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.DataObjects;
//using ReeperCommon.Gui.Logic;
//using ReeperCommon.Gui.Window;
//using ReeperCommon.Gui.Window.Buttons;
//using ReeperCommon.Gui.Window.Decorators;
//using ReeperCommon.Gui.Window.View;
//using UnityEngine;

//namespace AssemblyReloader.Gui
//{
//    class WindowFactory
//    {
//        private readonly IConfigurationPanelFactory _configurationPanelFactory;
//        private readonly IMessageChannel _viewMessageChannel;
//        private readonly GUISkin _windowSkin;
//        private readonly GUIStyle _titleBarButtonStyle;
//        private readonly Texture2D _closeButton;
//        private readonly Texture2D _optionsButton;
//        private readonly Texture2D _resizeCursor;
//// ReSharper disable once InconsistentNaming
//        private readonly Vector2 TitleBarButtonOffset = new Vector2(3f, 3f);


//        public WindowFactory(
//            [NotNull] IConfigurationPanelFactory configurationPanelFactory, 
//            [NotNull] IMessageChannel viewMessageChannel,
//            [NotNull] GUISkin windowSkin, 
//            [NotNull] GUIStyle titleBarButtonStyle,
//            [NotNull] Texture2D closeButton, 
//            [NotNull] Texture2D optionsButton, 
//            [NotNull] Texture2D resizeCursor)
//        {
//            if (configurationPanelFactory == null) throw new ArgumentNullException("configurationPanelFactory");
//            if (viewMessageChannel == null) throw new ArgumentNullException("viewMessageChannel");
//            if (windowSkin == null) throw new ArgumentNullException("windowSkin");
//            if (titleBarButtonStyle == null) throw new ArgumentNullException("titleBarButtonStyle");
//            if (closeButton == null) throw new ArgumentNullException("closeButton");
//            if (optionsButton == null) throw new ArgumentNullException("optionsButton");
//            if (resizeCursor == null) throw new ArgumentNullException("resizeCursor");

//            _configurationPanelFactory = configurationPanelFactory;
//            _viewMessageChannel = viewMessageChannel;
//            _windowSkin = windowSkin;
//            _titleBarButtonStyle = titleBarButtonStyle;
//            _closeButton = closeButton;
//            _optionsButton = optionsButton;
//            _resizeCursor = resizeCursor;
//        }



//        public void CreateMainWindow(
//            [NotNull] View logic, 
//            [NotNull] IWindowComponent programConfigurationWindow, 
//            [NotNull] ICommand saveProgramConfiguration,
//            Rect initialRect,
//            int winid)
//        {
//            if (logic == null) throw new ArgumentNullException("logic");
//            if (programConfigurationWindow == null) throw new ArgumentNullException("programConfigurationWindow");
//            if (saveProgramConfiguration == null) throw new ArgumentNullException("saveProgramConfiguration");

//            var basicWindow = new BasicWindow(logic, initialRect, winid, _windowSkin) { Title = "Assembly Reload Tool" };
//            var resizable = new Resizable(basicWindow, new Vector2(10f, 10f), new Vector2(150f, 100f), _resizeCursor);
//            var tbButtons = new TitleBarButtons(resizable, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);
            
//            var hiding = new HideOnF2(tbButtons);
//            var clamp = new ClampToScreen(hiding);

//            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, _optionsButton, s =>
//            {
//                programConfigurationWindow.Visible = !programConfigurationWindow.Visible;
//                saveProgramConfiguration.Execute();
//            }, "ProgramConfigurationButton"));

//            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, _closeButton, s =>
//            {
//                clamp.Visible = false;
//            }, "CloseMainWindow"));

//            programConfigurationWindow.Visible = false;

//            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(clamp, "MainWindow"));
//        }



//        public IWindowComponent CreatePluginOptionsWindow(
//            [NotNull] PluginConfigurationViewLogic pluginViewLogic,
//            [NotNull] IReloadablePlugin plugin,
//            [NotNull] ICommand saveOptionsCommand,
//            int winid)
//        {
//            if (pluginViewLogic == null) throw new ArgumentNullException("pluginViewLogic");
//            if (plugin == null) throw new ArgumentNullException("plugin");
//            if (saveOptionsCommand == null) throw new ArgumentNullException("saveOptionsCommand");


//            foreach (var panel in _configurationPanelFactory.CreatePanelsFor(plugin.Configuration))
//                pluginViewLogic.AddPanel(panel);

//            var basicWindow = CreateBasicWindow(pluginViewLogic, new Rect(300f, 300f, 450f, 300f),
//                winid, plugin.Name + " Configuration");


//            var tbButtons = CreateButtonToolbar(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

//            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle,
//                _closeButton,
//                s =>
//                {
//                    tbButtons.Visible = false;
//                    saveOptionsCommand.Execute();
//                }, "Close"));
            
//            //var decoratedWindow = new HideOnF2(new ClampToScreen(basicWindow)); //buggy?
//            var decoratedWindow = new ClampToScreen(tbButtons);

//            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decoratedWindow));

//            return decoratedWindow;
//        }


//        public IWindowComponent CreateProgramOptionsWindow([NotNull] ProgramConfigurationViewLogic logic,
//            [NotNull] Configuration programConfiguration, 
//            [NotNull] ICommand saveOptionsCommand,
//            int winid)
//        {
//            if (logic == null) throw new ArgumentNullException("logic");
//            if (programConfiguration == null) throw new ArgumentNullException("programConfiguration");
//            if (saveOptionsCommand == null) throw new ArgumentNullException("saveOptionsCommand");


//            var basicWindow = CreateBasicWindow(logic, new Rect(450f, 300f, 300f, 200f), winid,
//                "Program Configuration");

//            var tbButtons = CreateButtonToolbar(basicWindow, TitleBarButtons.ButtonAlignment.Right, TitleBarButtonOffset);

//            tbButtons.AddButton(new TitleBarButton(_titleBarButtonStyle, _closeButton,
//                s =>
//                {
//                    tbButtons.Visible = false;
//                    saveOptionsCommand.Execute();
//                }, "Close"));

//            var decorated = new ClampToScreen(tbButtons);

//            UnityEngine.Object.DontDestroyOnLoad(WindowView.Create(decorated));

//            return decorated;
//        }


//        private IWindowComponent CreateBasicWindow(IWindowLogic logic, Rect rect, int winid, string title)
//        {
//            return new BasicWindow(logic, rect, winid, _windowSkin) { Title = title};
//        }

//        private TitleBarButtons CreateButtonToolbar(IWindowComponent window,
//            TitleBarButtons.ButtonAlignment alignment, Vector2 offset)
//        {
//            return new TitleBarButtons(window, alignment, offset);
//        }
//    }
//}
