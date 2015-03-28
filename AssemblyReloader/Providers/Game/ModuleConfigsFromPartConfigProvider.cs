using System;
using System.Collections.Generic;
using System.Linq;

namespace AssemblyReloader.Providers.Game
{
    public class ModuleConfigsFromPartConfigProvider : IModuleConfigsFromPartConfigProvider
    {
        public IEnumerable<ConfigNode> Get(ConfigNode partConfig, string moduleName)
        {
            if (partConfig == null) throw new ArgumentNullException("partConfig");
            if (moduleName == null) throw new ArgumentNullException("moduleName");

            var allModules = partConfig.GetNodes("MODULE");

            return allModules.Where(cfg => cfg.HasValue("name") && cfg.GetValue("name") == moduleName);
        }
    }
}
