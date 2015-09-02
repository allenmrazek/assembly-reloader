using AssemblyReloader.Game;
using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleLoader
    {
        void Load(ILoadedAssemblyHandle handle);
    }
}
