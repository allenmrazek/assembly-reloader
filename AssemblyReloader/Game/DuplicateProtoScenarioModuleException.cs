using System;

namespace AssemblyReloader.Game
{
    public class DuplicateProtoScenarioModuleException : Exception
    {
        public DuplicateProtoScenarioModuleException(string scenarioModuleName)
            : base("A ProtoScenarioModule for type: " + scenarioModuleName + " already exists")
        {
            
        }
    }
}
