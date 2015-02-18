using System;
using System.Reflection;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PMLoader
{
    /// <summary>
    /// Every pseudo-PartModule will have a Proxy to manage its state and to prevent KSP from
    /// knowing it (using ProtoPartModule for instance)
    /// </summary>
    public class PartModuleProxy : PartModule
    {
        public ILog Log;
        public PartModule Real;
        public MethodInfo RealOnSave;

        private void Start()
        {
            if (Log.IsNull()) throw new ArgumentException("Log required");
            if (Real.IsNull()) throw new ArgumentException("Watched instance required");
        }

        private void OnPartPack()
        {
            Log.Debug("OnPartPack: " + Real.GetType().FullName);
        }

        private void OnPartUnpack()
        {
            Log.Debug("OnPartUnpack: " + Real.GetType().FullName);
        }


        public override void OnLoad(ConfigNode node)
        {
            base.OnLoad(node);
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);
        }
    }
}
