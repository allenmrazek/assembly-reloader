using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class MultipleScenarioModuleInstancesException : Exception
    {
        public MultipleScenarioModuleInstancesException()
            : base("Multiple ScenarioModule instances of type exist")
        {
        }

        public MultipleScenarioModuleInstancesException(string message) : base(message)
        {
        }

        public MultipleScenarioModuleInstancesException(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }

        public MultipleScenarioModuleInstancesException(Type scenarioModuleType)
            : base("More than one instance of " + scenarioModuleType.FullName + " exists!")
        {
            
        }
    }
}
