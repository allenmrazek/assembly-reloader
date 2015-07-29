using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.old
{
    public interface IPartModuleLoader
    {
        void Load(ILoadedAssemblyHandle handle);
    }
}
