using AssemblyReloader.ReloadablePlugin.Config;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IPluginConfigurationProvider
    {
        PluginConfiguration Get(IFile pluginLocation);
    }
}
