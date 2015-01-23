using System;
using System.Collections.Generic;
using AssemblyReloader.AssemblyTracking;
using AssemblyReloader.Logging;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Logic;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class MainViewLogic : IWindowLogic
    {
        private IWindowComponent _window;
        private readonly IReloadableController _controller;
        private readonly IWindowComponent _logWindow;

        // gui
        private Vector2 _scroll = default(Vector2);



        public MainViewLogic(IReloadableController controller, IWindowComponent logWindow)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            if (logWindow == null) throw new ArgumentNullException("logWindow");

            _controller = controller;
            _logWindow = logWindow;
        }




        public void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Reloadable assemblies:");

                DrawReloadableItems(_controller.ReloadableAssemblies);

                if (GUILayout.Button("Reload all"))
                    _controller.ReloadAll();

                //if (GUILayout.Button("Unload All"))
                //    _container.UnloadAll();

                _scroll = GUILayout.BeginScrollView(_scroll);
                {
                    for (int i = 0; i < 6; ++i)
                        GUILayout.Label("Item " + i);
                }
                GUILayout.EndScrollView();
                GUILayout.Space(10f);

                GUILayout.Box("Reload Options", GUILayout.MaxHeight(32f));
                GUILayout.Toggle(true, "KSPAddon");
                GUILayout.Toggle(true, "PartModule");
                GUILayout.Toggle(true, "ScenarioModule");

                if (GUILayout.Button(_logWindow.Visible ? "Hide Log" : "Show Log"))
                    _logWindow.Visible = !_logWindow.Visible;
            }
            GUILayout.EndVertical();
        }



        private void DrawReloadableItems(IEnumerable<IReloadableIdentity> items)
        {
            foreach (var item in items)
                DrawReloadableItem(item);
        }



        private void DrawReloadableItem(IReloadableIdentity reloadable)
        {
            GUILayout.BeginHorizontal(GUILayout.MinWidth(300f));
            {
                GUILayout.Label(reloadable.Name);
                GUILayout.FlexibleSpace();
                GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
            }
            GUILayout.EndHorizontal();
        }




        public void Update()
        {
            //_log.Normal("ViewLogic.Update");
        }



        public void OnAttached(IWindowComponent component)
        {
            _window = component;

        }

        public void OnDetached(IWindowComponent component)
        {

        }
    }
}
