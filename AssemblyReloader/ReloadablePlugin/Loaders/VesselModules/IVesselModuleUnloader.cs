using AssemblyReloader.Game;
using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleUnloader
    {
        void Unload(ILoadedAssemblyHandle handle);
    }
}
