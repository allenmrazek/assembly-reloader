using System.Collections.Generic;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public interface IReloadableAssemblyFileLocationQuery
    {
        IEnumerable<IFile> Get();
    }
}
