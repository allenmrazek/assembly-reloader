using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonLoader
    {
        void CreateAddons(ILoadedAssemblyHandle loadedAssembly);
    }
}
