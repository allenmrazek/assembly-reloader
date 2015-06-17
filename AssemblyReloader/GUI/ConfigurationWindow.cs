using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Gui.Messages;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class ConfigurationWindow : BasicWindow, IMessageConsumer<ShowConfigurationWindow>
    {
        private readonly Configuration _configuration;
        private readonly IController _controller;

        public ConfigurationWindow(
            [NotNull] Configuration configuration, 
            [NotNull] IController controller,
            Rect rect,
            int winid, 
            GUISkin skin, 
            bool draggable = true) : base(rect, winid, skin, draggable)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (controller == null) throw new ArgumentNullException("controller");
            _configuration = configuration;
            _controller = controller;
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            _configuration.ReloadAllReloadablesUponWindowFocus = GUILayout.Toggle(
                _configuration.ReloadAllReloadablesUponWindowFocus, "Reload all plugins upon window focus");

            if (GUI.changed)
                _controller.SaveConfiguration();
        }


        public void OnCloseButton()
        {
            Visible = false;
            
            _controller.SaveConfiguration();
        }


        public void Consume(ShowConfigurationWindow message)
        {
            Visible = true;
        }
    }
}
