using System;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ConfigNodeDoesNotSpecifyTypeException : Exception
    {
        public ConfigNodeDoesNotSpecifyTypeException(ConfigNode config)
            : base(
                string.Format("The following ConfigNode does not specify a type to instantiate: {0}", config.ToString())
                )
        {
            
        }
    }
}
