using System;
using AssemblyReloader.Annotations;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

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
            return _location.Directory.FileExists(new KSPUrlIdentifier(_location.FileName + DebugSymbolExtension)); 
        }
    }
}
