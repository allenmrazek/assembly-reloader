using System;
using System.Collections.Generic;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.Implementations
{
    public class ReloadableAssemblyFilesInDirectoryQuery : IReloadableAssemblyFileLocationQuery
    {
        private readonly IDirectory _topDirectory;

        public ReloadableAssemblyFilesInDirectoryQuery(IDirectory topDirectory)
        {
            if (topDirectory == null) throw new ArgumentNullException("topDirectory");
            _topDirectory = topDirectory;
        }


        public IEnumerable<IFile> Get()
        {
            return _topDirectory.RecursiveFiles("reloadable");
        }
    }
}
