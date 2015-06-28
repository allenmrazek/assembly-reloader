using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonFacadeFactory
    {
        IReloadableObjectFacade Create(PluginConfiguration pluginConfiguration);
    }
}