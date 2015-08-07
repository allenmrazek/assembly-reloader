using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class NoConfigNodeFoundException : Exception
    {
        public NoConfigNodeFoundException(Type receiver) : base("No ConfigNode found to use with " + receiver.FullName)
        {
            
        }
    }
}
