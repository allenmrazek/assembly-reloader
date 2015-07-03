using AssemblyReloader.Configuration;
using AssemblyReloader.DataObjects;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IPluginConfigurationProvider
    {
        PluginConfiguration Get(IFile pluginLocation);
    }
}
