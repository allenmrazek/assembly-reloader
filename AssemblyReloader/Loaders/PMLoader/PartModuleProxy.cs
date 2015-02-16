//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using ReeperCommon.Extensions;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.Game
//{
//    /// <summary>
//    /// Every pseudo-PartModule will have a Proxy to manage its state and to prevent KSP from
//    /// knowing it (using ProtoPartModule for instance)
//    /// </summary>
//    public class PartModuleProxy : PartModule
//    {
//        public ILog Log;
//        public PartModule Real;
//        public MethodInfo RealOnSave;

//        private void Start() 
//        {
//            if (Log.IsNull()) throw new ArgumentException("Log required");
//            if (Real.IsNull()) throw new ArgumentException("Watched instance required");
//        }

//        private void OnPartPack()
//        {
//            Log.Debug("OnPartPack: " + Real.GetType().FullName);
//        }

//        private void OnPartUnpack()
//        {
//            Log.Debug("OnPartUnpack: " + Real.GetType().FullName);
//        }
//    }
//}
