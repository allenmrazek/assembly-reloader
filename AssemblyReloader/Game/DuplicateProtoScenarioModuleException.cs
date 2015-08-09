using System;

namespace AssemblyReloader.Game
{
    public class DuplicateProtoScenarioModuleException : Exception
    {
        public DuplicateProtoScenarioModuleException(string scenarioModuleName)
            : base("A ProtoScenarioModule for type: " + scenarioModuleName + " already exists")
        {
            
        }

        public DuplicateProtoScenarioModuleException()
            : base("A duplicate ProtoScenarioModule exists for a type")
        {

        }


        public DuplicateProtoScenarioModuleException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
