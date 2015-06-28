using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IGetConfigurationFilePath
    {
        string Get(IFile pluginLocation);
    }
}
