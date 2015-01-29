using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Logging;
using ReeperCommon.Gui.Logic;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.GUI
{
    class LogViewLogic : IWindowLogic
    {
        private readonly ICachedLog _log;

        private IWindowComponent _window;
        private Vector2 _scroll = default(Vector2);



        public LogViewLogic(ICachedLog log)
        {
            if (log == null) throw new ArgumentNullException("log");

            _log = log;
        }



        public void Draw()
        {
            GUILayout.BeginVertical();
            DrawLog(_log);
            GUILayout.EndVertical();
        }



        public void Update()
        {

        }



        public void OnAttached(IWindowComponent component)
        {
            _window = component;
        }



        public void OnDetached(IWindowComponent component)
        {
            _window = null;
        }


        private void DrawLog(ICachedLog log)
        {
            _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.MinHeight(300f), GUILayout.MinWidth(400f));
            {
                foreach (var msg in log.Messages)
                    GUILayout.Label(msg);
            }
            GUILayout.EndScrollView();
        }
    }
}
