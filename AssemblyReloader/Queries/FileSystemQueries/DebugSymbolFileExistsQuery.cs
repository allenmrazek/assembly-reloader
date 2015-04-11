using System;
using System.IO;
using AssemblyReloader.Annotations;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class DebugSymbolFileExistsQuery : IDebugSymbolFileExistsQuery
    {
        private const string DebugSymbolExtension = ".mdb";

        private readonly IFile _location;

        public DebugSymbolFileExistsQuery([NotNull] IFile location)
        {
            if (location == null) throw new ArgumentNullException("location");
            _location = location;
        }


        public bool Get()
        {
            // note: we use the actual file system instead of checking cached files from GameDatabase
            // because GameDatabase will only contain the state of the file at program load; the debug
            // symbol file might come and go during art's lifetime
            return File.Exists(_location.FileName + DebugSymbolExtension);
        }
    }
}
