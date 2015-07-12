using System;
using System.Collections.Generic;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Config
{
    public class GetReloadableAssemblyFilesFromDirectoryRecursive
    {
        private readonly IDirectory _topDirectory;

        public GetReloadableAssemblyFilesFromDirectoryRecursive(IDirectory topDirectory)
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
