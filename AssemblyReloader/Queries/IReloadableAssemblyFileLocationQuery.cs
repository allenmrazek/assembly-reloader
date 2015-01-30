using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries
{
    public interface IReloadableAssemblyFileLocationQuery
    {
        IEnumerable<IFile> Get();
    }
}
