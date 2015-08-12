extern alias KSP;
using System.Collections.Generic;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IGetPartModuleConfigsFromPartConfig
    {
        IEnumerable<KSP::ConfigNode> Get(KSP::ConfigNode partConfig, string moduleName);
    }
}
