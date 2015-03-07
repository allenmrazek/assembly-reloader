using System;
using System.Collections.Generic;
using AssemblyReloader.Controllers;
using AssemblyReloader.Logging;
using AssemblyReloader.PluginTracking;
using ReeperCommon.Gui.Logic;
using ReeperCommon.Gui.Window;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class MainViewWindowLogic : IWindowLogic
    {
        private readonly IReloadablePluginController _pluginController;
        private readonly IWindowComponent _logWindow;

        // gui
        private Vector2 _scroll = default(Vector2);



        public MainViewWindowLogic(IReloadablePluginController pluginController, IWindowComponent logWindow)
        {
            if (pluginController == null) throw new ArgumentNullException("pluginController");
            if (logWindow == null) throw new ArgumentNullException("logWindow");

            _pluginController = pluginController;
            _logWindow = logWindow;
        }




        public void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Reloadable assemblies:");

                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.MinWidth(250f), GUILayout.MinHeight(200f));
                {
                    DrawReloadableItems(_pluginController.Plugins);
                }
                GUILayout.EndScrollView();

                if (GUILayout.Button("Reload all"))
                    _pluginController.ReloadAll();

                GUILayout.Space(10f);

                if (GUILayout.Button(_logWindow.Visible ? "Hide Log" : "Show Log"))
                    _logWindow.Visible = !_logWindow.Visible;

                GUILayout.Toggle(true, "Re-create KSPAddons for this scene");
            }
            GUILayout.EndVertical();
        }



        private void DrawReloadableItems(IEnumerable<IReloadablePlugin> items)
        {
            foreach (var item in items)
                DrawReloadableItem(item);
        }



        private void DrawReloadableItem(IReloadablePlugin reloadable)
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(reloadable.Name);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                    _pluginController.Reload(reloadable);
            }
            GUILayout.EndHorizontal();
        }




        public void Update()
        {
            //_log.Normal("ViewLogic.Update");
        }

    }
}
