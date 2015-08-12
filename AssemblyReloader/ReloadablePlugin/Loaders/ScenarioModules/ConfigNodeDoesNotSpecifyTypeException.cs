extern alias KSP;
using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ConfigNodeDoesNotSpecifyTypeException : Exception
    {
        public ConfigNodeDoesNotSpecifyTypeException()
            : base("ConfigNode does not contain a name value")
        {
            
        }


        public ConfigNodeDoesNotSpecifyTypeException(string message) : base(message)
        {
        }

        public ConfigNodeDoesNotSpecifyTypeException(string message, Exception innerException)
            : base(message, innerException)
        {
        }


        public ConfigNodeDoesNotSpecifyTypeException(KSP::ConfigNode config)
            : base(
                string.Format("The following ConfigNode does not specify a type to instantiate: {0}", config.ToString())
                )
        {
            
        }
    }
}
