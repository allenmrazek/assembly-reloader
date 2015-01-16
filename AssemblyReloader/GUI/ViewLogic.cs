using System;
using AssemblyReloader.AssemblyTracking;
using AssemblyReloader.AssemblyTracking.Implementations;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Logic;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class ViewLogic : IWindowLogic
    {
        private IWindowComponent _window;
        private readonly IReloadableController _controller;
        private readonly ILog _log;

        // gui
        private Vector2 _scroll = default(Vector2);


        public ViewLogic(IReloadableController controller, ILog log)
        {
            if (controller == null) throw new ArgumentNullException("controller");
            if (log == null) throw new ArgumentNullException("log");

            _controller = controller;
            _log = log;

            _log.Debug("ViewLogic created");
        }




        public void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Reloadable assemblies:");

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
            }
            GUILayout.EndVertical();
        }



        public void Update()
        {
            //_log.Normal("ViewLogic.Update");
        }



        public void OnAttached(IWindowComponent component)
        {
            _window = component;
            _log.Normal("ViewLogic.OnAttached");

        }

        public void OnDetached(IWindowComponent component)
        {
            _log.Normal("ViewLogic.OnDetached");
        }
    }
}
