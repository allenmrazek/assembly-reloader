using System;
using ReeperCommon.Gui.Window;
using ReeperCommon.Gui.Window.Logic;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class ViewLogic : IWindowLogic
    {
        private IWindowComponent _window;
        private readonly Log _log;

        // gui
        private Vector2 _scroll = default(Vector2);


        public ViewLogic(Log log)
        {
            if (log == null) throw new ArgumentNullException("log");
            _log = log;

            _log.Normal("ViewLogic created");
        }




        public void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Reloadable assemblies:");

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
            _log.Normal("ViewLogic.Update");
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
