﻿using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
using ReeperCommon.Extensions;
using ReeperCommon.Gui.Window;
using UnityEngine;

namespace AssemblyReloader.Gui
{
    public class StrangeWindowView : EventView
    {
        [Inject]
// ReSharper disable once MemberCanBePrivate.Global
// ReSharper disable once UnusedAutoPropertyAccessor.Global
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