using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class NoScenarioModuleReferenceException : Exception
    {
        public NoScenarioModuleReferenceException(IProtoScenarioModule protoScenarioModule)
            : base(
                "ProtoScenarioModule for " + protoScenarioModule.moduleName +
                " does not contain a live ScenarioModule reference")
        {
            
        }
    }
}
