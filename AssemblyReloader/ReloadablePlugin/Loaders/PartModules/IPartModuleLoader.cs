using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleLoader
    {
        void Load(ILoadedAssemblyHandle handle);
    }
}
