using AssemblyReloader.DataObjects;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IAddonFacadeFactory
    {
        IReloadableObjectFacade Create(PluginConfiguration pluginConfiguration);
    }
}