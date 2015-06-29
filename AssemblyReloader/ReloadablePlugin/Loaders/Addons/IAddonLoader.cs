using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonLoader
    {
        void Load(ILoadedAssemblyHandle loadedAssembly);
    }
}
