using ReeperKSP.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    public interface IGetConfigurationFilePath
    {
        string Get(IFile pluginLocation);
    }
}
