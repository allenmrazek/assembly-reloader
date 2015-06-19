using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class StrangeWindowView : View
    {
        [Inject]
// ReSharper disable once MemberCanBePrivate.Global
        public IWindowComponent Logic { get; set; }



// ReSharper disable once UnusedMember.Local
// ReSharper disable once InconsistentNaming
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



// ReSharper disable once UnusedMember.Local
        private void Update()
        {
            if (Logic.IsNull()) return;

            Logic.Update();
        }
    }
}
