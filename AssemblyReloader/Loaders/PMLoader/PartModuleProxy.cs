using System;
using System.Reflection;
using AssemblyReloader.PluginTracking;
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
        public PartModule TargetInstance;
        public Type TargetType;
        public ReloadablePlugin TargetAssembly;

        public MethodInfo RealOnSave;

        private void Start()
        {
            if (Log.IsNull()) throw new ArgumentException("Log required");
            if (TargetInstance.IsNull()) throw new ArgumentException("Target instance required");
        }

        private void OnPartPack()
        {
            Log.Debug("OnPartPack: " + TargetType.FullName);
        }

        private void OnPartUnpack()
        {
            Log.Debug("OnPartUnpack: " + TargetType.FullName);
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
