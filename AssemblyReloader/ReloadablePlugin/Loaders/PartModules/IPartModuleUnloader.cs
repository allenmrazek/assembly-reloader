using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleUnloader
    {
        void Unload(ILoadedAssemblyHandle handle);
    }
}
