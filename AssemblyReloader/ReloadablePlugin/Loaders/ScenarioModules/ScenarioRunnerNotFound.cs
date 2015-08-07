using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ScenarioRunnerNotFound : Exception
    {
        public ScenarioRunnerNotFound() : base("KSP ScenarioRunner not found")
        {
            
        }
    }
}
