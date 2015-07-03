using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IGetAssemblyFileLocation
    {
        Maybe<IFile> Get(Assembly target);
    }
}
