using System;
using System.Collections.Generic;
using AssemblyReloader.Properties;
using ReeperCommon.Gui.Window;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class MainWindowLogic : BasicWindowLogic, IMainView
    {
        private IEnumerable<IPluginInfo> _plugins;
        private Vector2 _scroll = default(Vector2);

        [Inject]
        public ILog Log { get; set; }


        [Inject]
        public IEnumerable<IPluginInfo> Plugins
        {
            get { return _plugins; }
            set { _plugins = value; }
        }


        public MainWindowLogic(Rect rect, int winid, GUISkin skin, bool draggable = true)
            : base(rect, winid, skin, draggable)
        {
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    foreach (var item in _plugins)
                        DrawReloadableItem(item);
                }
                GUILayout.EndScrollView();
            }
            GUILayout.EndVertical();
        }




        private void DrawReloadableItem([NotNull] IPluginInfo reloadable)
        {
            if (reloadable == null) throw new ArgumentNullException("reloadable");

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(reloadable.Name);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Reload", GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false)))
                {
                    //_controller.Reload(reloadable);
                }
            }
            GUILayout.EndHorizontal();
        }
    }
}
