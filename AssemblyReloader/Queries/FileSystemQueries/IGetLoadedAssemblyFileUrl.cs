using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IGetLoadedAssemblyFileUrl
    {
        string Get(IFile assemblyLocation);
    }
}
