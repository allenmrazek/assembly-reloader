using System;
using System.Collections.Generic;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

namespace AssemblyReloader.Queries
{
    class ReloadableAssemblyFileQuery
    {
        private readonly IDirectory _topDirectory;

        public ReloadableAssemblyFileQuery(IDirectory topDirectory)
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
