using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleUnloader
    {
        void Unload(ILoadedAssemblyHandle handle);
    }
}
