using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class MultipleScenarioModuleInstancesException : Exception
    {
        public MultipleScenarioModuleInstancesException(Type scenarioModuleType)
            : base("More than one instance of " + scenarioModuleType.FullName + " exists!")
        {
            
        }
    }
}
