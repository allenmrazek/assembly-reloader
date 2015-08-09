using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class NoConfigNodeFoundException : Exception
    {
        public NoConfigNodeFoundException() : base("No ConfigNode found")
        {
        }

        public NoConfigNodeFoundException(string message) : base(message)
        {
        }

        public NoConfigNodeFoundException(string message, Exception innerException) : base(message, innerException)
        {
            
        }

        public NoConfigNodeFoundException(Type receiver) : base("No ConfigNode found to use with " + receiver.FullName)
        {
            
        }
    }
}
