using System;
using System.Collections.Generic;
using AssemblyReloader.Properties;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Config
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowLogic : BasicWindowLogic
    {
        private readonly MainView _view;
        private Vector2 _scroll = default(Vector2);

        public IEnumerable<IPluginInfo> Plugins { get; set; }


        public MainWindowLogic(MainView view)
            : base(new Rect(400f, 400f, 400f, 400f), new WindowID(), HighLogic.Skin, true)
        {
            if (view == null) throw new ArgumentNullException("view");

            _view = view; // mediator doesn't know about us, MainWindowLogic. But we need to communicate
            // with it. View will define what we can say to mediator (and what we can hear)
            // and we'll trigger that communication from here
        }



        public override void OnWindowDraw(int winid)
        {
            base.OnWindowDraw(winid);

            GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                {
                    foreach (var item in Plugins)
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
                    //Mediator.Reload(reloadable);
                }
            }
            GUILayout.EndHorizontal();
        }


        public void ToggleSettings()
        {
            //Mediator.ToggleOptions();
        }


        public void Close()
        {
            //Mediator.HideMainWindow();
        }
    }
}
