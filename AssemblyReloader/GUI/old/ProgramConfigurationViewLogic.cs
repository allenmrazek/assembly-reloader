//using System;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.DataObjects;
//using ReeperCommon.Gui.Logic;
//using UnityEngine;

//namespace AssemblyReloader.Gui
//{
//    public class ProgramConfigurationViewLogic : IWindowLogic
//    {
//        private readonly Configuration _configuration;

//        public ProgramConfigurationViewLogic([NotNull] Configuration configuration)
//        {
//            if (configuration == null) throw new ArgumentNullException("configuration");

//            _configuration = configuration;
//        }


//        public void Draw()
//        {
//            _configuration.ReloadAllReloadablesUponWindowFocus = GUILayout.Toggle(
//                _configuration.ReloadAllReloadablesUponWindowFocus, "Reload all plugins upon window focus");
//        }


//        public void Update()
//        {
            
//        }
//    }
//}
