using AssemblyReloader.Config;
using AssemblyReloader.DataObjects;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Gui
{
// ReSharper disable once UnusedMember.Global
    public class SettingsWindowLogic : BasicWindowLogic, ISettingsView
    {
        [Inject] public IViewMediator Mediator { get; set; }
        [Inject] public Configuration Configuration { get; set; }

        public SettingsWindowLogic()
            : base(new Rect(400f, 400f, 400f, 400f), new WindowID(), HighLogic.Skin, true)
        {
        }


        [PostConstruct]
        // ReSharper disable once UnusedMember.Local
        public void LinkToMediator()
        {
            Mediator.SettingsView = this;
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            //Configuration.ReloadAllReloadablesUponWindowFocus = GUILayout.Toggle(
            //    Configuration.ReloadAllReloadablesUponWindowFocus, "Reload all plugins upon window focus");

            GUILayout.Label("Data goes here");
            // todo: save configuration
            //if (GUI.changed)
            //    Configuration.SaveConfiguration();
        }


        public void Close()
        {
            Visible = false;
            Mediator.SaveConfiguration();
        }
    }
}
