using System;
using System.Linq;
using AssemblyReloader.Annotations;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;
using ReeperCommon.Logging.Implementations;

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
            var urlid = new KSPUrlIdentifier(_location.FileName + DebugSymbolExtension);

            var result2 = _location.Directory.FileExists(new KSPUrlIdentifier("TestProject.dll.reloadable.mdb"));

            var files = _location.Directory.Files().ToList();

            var result = _location.Directory.FileExists(new KSPUrlIdentifier(_location.FileName + DebugSymbolExtension));

            new DebugLog().Normal("DebugSymbolFileExists for " + _location.FileName + ": " + result);

            return result;
        }
    }
}
