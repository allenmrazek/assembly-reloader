//using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
//using ReeperCommon.Gui;
//using ReeperCommon.Gui.Window;
//using UnityEngine;

//namespace AssemblyReloader.Config
//{
//// ReSharper disable once UnusedMember.Global
//    public class SettingsWindowLogic : View, IWindowComponent
//    {
//        public Configuration Configuration { get; set; }

//        public SettingsWindowLogic()
//            : base(new Rect(400f, 400f, 400f, 400f), new WindowID(), HighLogic.Skin, true)
//        {
//        }

//        protected override void Awake()
//        {
//            base.Awake();
//        }


//        private void OnGUI()
//        {
//            if (Logic.IsNull() || !Logic.Visible) return;

//            if (!Logic.Skin.IsNull())
//                GUI.skin = Logic.Skin;


//            Logic.Dimensions = GUILayout.Window(Logic.Id.Value, Logic.Dimensions, DrawWindow,
//                Logic.Title);

//        }


//        private void DrawWindow(int winid)
//        {
//            Logic.OnWindowDraw(winid);
//            Logic.OnWindowFinalize(winid);
//        }


//        // ReSharper disable once UnusedMember.Global
//        private void Update()
//        {
//            if (Logic.IsNull()) return;

//            Logic.Update();
//        }
//    }
//}
