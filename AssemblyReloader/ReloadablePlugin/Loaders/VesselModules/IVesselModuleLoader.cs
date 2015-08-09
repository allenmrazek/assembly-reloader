using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleLoader
    {
        void Load(ILoadedAssemblyHandle handle);
    }
}
