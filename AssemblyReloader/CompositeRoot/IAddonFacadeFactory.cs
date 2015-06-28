using AssemblyReloader.DataObjects;
using AssemblyReloader.ReloadablePlugin;

namespace AssemblyReloader.CompositeRoot
{
    public interface IAddonFacadeFactory
    {
        IReloadableObjectFacade Create(PluginConfiguration pluginConfiguration);
    }
}