using System;
using System.Collections.Generic;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

namespace AssemblyReloader.Queries
{
    class ReloadableAssemblyFileQuery
    {
        private readonly IFileFactory _fileFactory;
        private readonly IGameDataPathQuery _gdQuery;

        public ReloadableAssemblyFileQuery(IFileFactory fileFactory, IGameDataPathQuery gdQuery)
        {
            if (fileFactory == null) throw new ArgumentNullException("fileFactory");
            if (gdQuery == null) throw new ArgumentNullException("gdQuery");

            _gdQuery = gdQuery;
            _fileFactory = fileFactory;
        }



        public IEnumerable<IFile> Get()
        {
            var d = new KSPDirectory(_fileFactory, _gdQuery);

            return d.RecursiveFiles("reloadable");
        }
    }
}
