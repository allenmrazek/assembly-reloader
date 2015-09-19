using System;
using System.Linq;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ModuleRefAlreadyExistsException : Exception
    {
        public ModuleRefAlreadyExistsException() : base("ModuleRef already exists")
        {
        }

        public ModuleRefAlreadyExistsException(string message) : base(message)
        {
        }

        public ModuleRefAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ModuleRefAlreadyExistsException(IProtoScenarioModule psm)
            : base(
                "ProtoScenarioModule for " + psm.moduleName + " already contains a ScenarioModule reference of " +
                psm.moduleRef.Single().GetType().FullName)
        {
            
        }
    }
}
