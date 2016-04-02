using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetPartModuleConfigsFromPartConfig
    {
        IEnumerable<ConfigNode> Get(ConfigNode partConfig, string moduleName);
    }
}
