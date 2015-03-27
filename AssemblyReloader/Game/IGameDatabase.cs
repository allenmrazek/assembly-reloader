using System.Collections.Generic;

namespace AssemblyReloader.Game
{
    public interface IGameDatabase
    {
        IEnumerable<UrlDir.UrlConfig> GetConfigs(string typeName);
    }
}
