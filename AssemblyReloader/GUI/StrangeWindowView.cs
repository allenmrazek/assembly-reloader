using System;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class StrangeWindowView : View
    {
        [Inject]
        public IWindowComponent Logic { get; set; }



        private void OnGUI()
        {
            if (Logic.IsNull() || !Logic.Visible) return;

            if (!Logic.Skin.IsNull())
                GUI.skin = Logic.Skin;


            Logic.Dimensions = GUILayout.Window(Logic.Id, Logic.Dimensions, DrawWindow,
                Logic.Title);

        }


        private void DrawWindow(int winid)
        {
            Logic.OnWindowDraw(winid);
            Logic.OnWindowFinalize(winid);
        }


        // ReSharper disable once UnusedMember.Global
        private void Update()
        {
            if (Logic.IsNull()) return;

            Logic.Update();
        }


        public static StrangeWindowView Create(IWindowComponent window, string goName = "WindowView")
        {
            if (window == null) throw new ArgumentNullException("window");

            var view = new GameObject(goName).AddComponent<StrangeWindowView>();
            view.Logic = window;

            return view;
        }
    }
}
