extern alias KSP;
using System.Collections.Generic;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IGetAddonTypesForScene
    {
        IEnumerable<ReloadableAddonType> Get(KSP::KSPAddon.Startup scene,
            ILoadedAssemblyHandle handle);
    }
}
