using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonFacadeFactory
    {
        IReloadableTypeSystem Create(PluginConfiguration pluginConfiguration);
    }
}