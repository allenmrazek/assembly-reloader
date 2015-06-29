using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonUnloader
    {
        void DestroyAddons(ILoadedAssemblyHandle assemblyHandle);
    }
}
