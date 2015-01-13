using System;
using AssemblyReloader.AddonTracking;
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
        private readonly ReloadableContainer _container;
        private readonly Log _log;

        // gui
        private Vector2 _scroll = default(Vector2);


        public ViewLogic(ReloadableContainer container, Log log)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (log == null) throw new ArgumentNullException("log");

            _container = container;
            _log = log;

            _log.Verbose("ViewLogic created");
        }




        public void Draw()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.Label("Reloadable assemblies:");

                if (GUILayout.Button("Reload all"))
                    _container.ReloadAllAssemblies();

                if (GUILayout.Button("Unload All"))
                    _container.UnloadAll();

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
