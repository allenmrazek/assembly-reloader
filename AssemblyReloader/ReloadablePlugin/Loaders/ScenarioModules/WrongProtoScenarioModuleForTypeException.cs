using System;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class WrongProtoScenarioModuleForTypeException : Exception
    {
        public WrongProtoScenarioModuleForTypeException(IProtoScenarioModule psmGiven, ITypeIdentifier identifier)
            : base(
                "Given ProtoScenarioModule is for " + psmGiven.moduleName + "; attempted to supply it " +
                identifier.Identifier)
        {
            
        }
    }
}
