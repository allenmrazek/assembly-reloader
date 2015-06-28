using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IPluginConfigurationFilePathQuery
    {
        string Get(IFile pluginLocation);
    }
}
