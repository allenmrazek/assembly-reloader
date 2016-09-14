using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGameDatabase
    {
        IEnumerable<UrlDir.UrlConfig> GetConfigs(string typeName);
    }
}
