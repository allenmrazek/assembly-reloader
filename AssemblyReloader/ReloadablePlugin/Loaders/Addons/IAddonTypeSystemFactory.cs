using AssemblyReloader.Config;
using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonTypeSystemFactory
    {
        IReloadableTypeSystem Create(PluginConfiguration pluginConfiguration);
    }
}