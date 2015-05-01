using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface ILoadedAssemblyFileUrlQuery
    {
        string Get(IFile assemblyLocation);
    }
}
