using AssemblyReloader.DataObjects;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Providers
{
    public interface IPluginConfigurationProvider
    {
        Configuration Get(IFile pluginLocation);
    }
}
