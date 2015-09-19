using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class WrongProtoScenarioModuleForTypeException : Exception
    {
        public WrongProtoScenarioModuleForTypeException() : base("Wrong ProtoScenarioModule for this type")
        {
 
        }

        public WrongProtoScenarioModuleForTypeException(string message) : base(message)
        {
            
        }

        public WrongProtoScenarioModuleForTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public WrongProtoScenarioModuleForTypeException(IProtoScenarioModule psmGiven, TypeIdentifier identifier)
            : base(
                "Given ProtoScenarioModule is for " + psmGiven.moduleName + "; attempted to supply it " +
                identifier.Identifier)
        {
            
        }
    }
}
