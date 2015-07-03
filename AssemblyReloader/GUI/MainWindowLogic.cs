using System;
using System.Collections.Generic;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Gui;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Gui
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class MainWindowLogic : BasicWindowLogic, IMainView
    {
        private Vector2 _scroll = default(Vector2);

        [Inject] public IEnumerable<IPluginInfo> Plugins { get; set; }
        [Inject] public IViewMediator Mediator { get; set; }


        public MainWindowLogic()
            : base(new Rect(400f, 400f, 400f, 400f), new WindowID(), HighLogic.Skin, true)
        {

        }


        [PostConstruct]
// ReSharper disable once UnusedMember.Global
        public void AttachToMediator()
        {
            Mediator.MainView = this;
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
                    Mediator.Reload(reloadable);
            }
            GUILayout.EndHorizontal();
        }


        public void ToggleSettings()
        {
            Mediator.ToggleOptions();
        }


        public void Close()
        {
            Mediator.HideMainWindow();
        }
    }
}
