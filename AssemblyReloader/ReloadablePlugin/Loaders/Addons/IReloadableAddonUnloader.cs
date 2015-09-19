using ReeperAssemblyLibrary;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IReloadableAddonUnloader
    {
        int DestroyAddons(ILoadedAssemblyHandle handle);
    }
}
