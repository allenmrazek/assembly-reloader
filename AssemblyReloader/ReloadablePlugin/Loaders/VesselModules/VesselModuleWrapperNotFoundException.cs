using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public class VesselModuleWrapperNotFoundException : Exception
    {
        public VesselModuleWrapperNotFoundException() : base("VesselModuleWrapper not found")
        {
            
        }

        public VesselModuleWrapperNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public VesselModuleWrapperNotFoundException(VesselModuleManager.VesselModuleWrapper wrapper)
            : base("VesselModuleWrapper for " + wrapper.type.FullName + " not found")
        {
            
        }

        public VesselModuleWrapperNotFoundException(string message) : base(message)
        {
            
        }
    }
}
