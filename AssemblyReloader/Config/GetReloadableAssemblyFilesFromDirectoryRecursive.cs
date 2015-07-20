using System;
using System.Collections.Generic;
using AssemblyReloader.Config.Keys;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Config
{
    [Implements(typeof(GetReloadableAssemblyFilesFromDirectoryRecursive))]
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetReloadableAssemblyFilesFromDirectoryRecursive
    {
        private readonly IDirectory _topDirectory;

        public GetReloadableAssemblyFilesFromDirectoryRecursive([Name(DirectoryKeys.GameData)] IDirectory topDirectory)
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
