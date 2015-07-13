//using System.Collections.Generic;
//using AssemblyReloader.StrangeIoC.extensions.mediation.impl;
//using ReeperCommon.Extensions;
//using ReeperCommon.Gui.Window;
//using UnityEngine;

//namespace AssemblyReloader.Config
//{
//// ReSharper disable once UnusedMember.Global
//    public class MainView : View
//    {
//        private IWindowComponent _logic;

//        public IEnumerable<IPluginInfo> Plugins { get; set; }


//        protected override void Awake()
//        {
//            base.Awake();
//            print("MainView.Awake");
//            _logic = WindowFactory.CreateMainWindow(this);
//            print(_logic == null ? "logic failed" : "logic success");
//        }


//        // ReSharper disable once InconsistentNaming
//        // ReSharper disable once UnusedMember.Global
//        private void OnGUI()
//        {
//            if (!_logic.Skin.IsNull())
//                GUI.skin = _logic.Skin;

//            _logic.Dimensions = GUILayout.Window(_logic.Id.Value, _logic.Dimensions, DrawWindow,
//                _logic.Title);

            
//        }


//        private void DrawWindow(int winid)
//        {
//            _logic.OnWindowDraw(winid);
//            _logic.OnWindowFinalize(winid);
//        }


//        // ReSharper disable once UnusedMember.Global
//        private void Update()
//        {
//            _logic.Update();
//        }
//    }
//}
