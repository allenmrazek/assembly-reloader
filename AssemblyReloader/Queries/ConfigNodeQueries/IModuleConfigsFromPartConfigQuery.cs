using System.Collections.Generic;

namespace AssemblyReloader.Queries.ConfigNodeQueries
{
    public interface IModuleConfigsFromPartConfigQuery
    {
        IEnumerable<ConfigNode> Get(ConfigNode partConfig, string moduleName);
    }
}
