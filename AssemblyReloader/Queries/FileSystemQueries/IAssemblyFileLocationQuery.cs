using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IAssemblyFileLocationQuery
    {
        Maybe<IFile> Get(Assembly target);
    }
}
