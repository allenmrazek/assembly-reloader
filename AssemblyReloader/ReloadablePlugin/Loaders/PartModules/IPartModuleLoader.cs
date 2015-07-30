using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleLoader
    {
        void LoadPrefabs(ILoadedAssemblyHandle handle);
        void LoadInstances(ILoadedAssemblyHandle handle);
    }
}
