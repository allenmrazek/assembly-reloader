using System;
using System.Collections.Generic;
using ReeperCommon.Gui.Window;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class MainWindowLogic : BasicWindowLogic, IMainView
    {
        private IEnumerable<IPluginInfo> _plugins;

        [Inject]
        public ILog Log { get; set; }


        public MainWindowLogic(Rect rect, int winid, GUISkin skin, bool draggable = true)
            : base(rect, winid, skin, draggable)
        {
        }


        [PostConstruct]
        private void TestSituation()
        {
            Log.Normal("MainWindowLogic created");
            throw new Exception("TestSituation");
        }


        public IEnumerable<IPluginInfo> Plugins
        {
            get { return _plugins; }
            set { _plugins = value; }
        }


        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                //_scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                //{
                //    DrawReloadableItems(_plugins);
                //}
                //GUILayout.EndScrollView();
                GUILayout.Button("Hello world", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            }
            GUILayout.EndVertical();
        }
    }
}
