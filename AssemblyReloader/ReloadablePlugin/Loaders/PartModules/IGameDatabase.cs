extern alias KSP;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGameDatabase
    {
        IEnumerable<KSP::UrlDir.UrlConfig> GetConfigs(string typeName);
    }
}
