using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IConfigurationFilePathQuery
    {
        string Get(IFile pluginLocation);
    }
}
