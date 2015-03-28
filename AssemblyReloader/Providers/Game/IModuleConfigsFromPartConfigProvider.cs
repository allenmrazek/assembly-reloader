using System.Collections.Generic;

namespace AssemblyReloader.Providers.Game
{
    public interface IModuleConfigsFromPartConfigProvider
    {
        IEnumerable<ConfigNode> Get(ConfigNode partConfig, string moduleName);
    }
}
