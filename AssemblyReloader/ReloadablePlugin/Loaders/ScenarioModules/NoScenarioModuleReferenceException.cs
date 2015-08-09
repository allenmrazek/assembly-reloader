using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class NoScenarioModuleReferenceException : Exception
    {
        public NoScenarioModuleReferenceException() : base("No ScenarioModule reference")
        {
        }

        public NoScenarioModuleReferenceException(string message) : base(message)
        {
        }

        public NoScenarioModuleReferenceException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public NoScenarioModuleReferenceException(IProtoScenarioModule protoScenarioModule)
            : base(
                "ProtoScenarioModule for " + protoScenarioModule.moduleName +
                " does not contain a live ScenarioModule reference")
        {
            
        }
    }
}
