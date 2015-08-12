extern alias KSP;
using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public class VesselModuleType
    {
        public Type Type { get; private set; }


        public VesselModuleType(Type vmType)
        {
            if (vmType == null) throw new ArgumentNullException("vmType");

            if (!vmType.IsSubclassOf(typeof(KSP::VesselModule)))
                throw new ArgumentException(vmType.FullName + " is not a VesselModule", "vmType");

            Type = vmType;
        }


        public override string ToString()
        {
            return "VesselModuleType: " + Type.FullName;
        }
    }
}
