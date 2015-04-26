using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IConfigurationFilePathQuery
    {
        Maybe<string> Get(IFile pluginLocation);
    }
}
