using System.Collections.Generic;

namespace AssemblyReloader.Game.Queries
{
    public interface IModuleConfigsFromPartConfigQuery
    {
        IEnumerable<ConfigNode> Get(ConfigNode partConfig, string moduleName);
    }
}
