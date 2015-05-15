using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IPluginConfigurationFilePathQuery
    {
        string Get(IFile pluginLocation);
    }
}
