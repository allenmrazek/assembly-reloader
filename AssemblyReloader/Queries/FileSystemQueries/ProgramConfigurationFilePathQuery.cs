using System;
using System.IO;
using AssemblyReloader.Annotations;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class ProgramConfigurationFilePathQuery : IProgramConfigurationFilePathQuery
    {
        private readonly IFile _reloaderDll;
        private const string ProgramConfigurationFileExtension = ".config";

        public ProgramConfigurationFilePathQuery([NotNull] IFile reloaderDll)
        {
            if (reloaderDll == null) throw new ArgumentNullException("reloaderDll");
            _reloaderDll = reloaderDll;
        }


        public string Get()
        {
            var location = _reloaderDll.FullPath;
            if (string.IsNullOrEmpty(location)) throw new FileNotFoundException(location);

            return Path.Combine(Path.GetDirectoryName(location) ?? string.Empty, Path.GetFileNameWithoutExtension(location) + ProgramConfigurationFileExtension);
        }
    }
}
