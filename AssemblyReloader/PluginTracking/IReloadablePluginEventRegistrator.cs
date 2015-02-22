using AssemblyReloader.Loaders.AddonLoader;

namespace AssemblyReloader.PluginTracking
{
    public interface IReloadablePluginEventRegistrator
    {
        void RegisterPluginLoad(IReloadablePlugin plugin, IAddonLoader addonLoader);
        void RegisterPluginUnload(IReloadablePlugin plugin, IAddonLoader addonLoader);
    }
}
