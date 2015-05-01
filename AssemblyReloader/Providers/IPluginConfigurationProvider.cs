using AssemblyReloader.DataObjects;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public interface IPluginConfigurationProvider
    {
        PluginConfiguration Get(IFile pluginLocation);
    }
}
