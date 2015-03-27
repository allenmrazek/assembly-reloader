using System.Collections.Generic;

namespace AssemblyReloader.Queries
{
    public interface IModuleConfigsFromPartConfigQuery
    {
        IEnumerable<ConfigNode> Get(ConfigNode partConfig, string moduleName);
    }
}
