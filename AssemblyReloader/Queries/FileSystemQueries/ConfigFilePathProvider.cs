using System;
using System.IO;
using AssemblyReloader.Annotations;
using AssemblyReloader.Providers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class ConfigFilePathProvider : IFilePathProvider
    {
        private readonly IFile _associatedDll;
        private const string ProgramConfigurationFileExtension = ".config";

        public ConfigFilePathProvider([NotNull] IFile associatedDll)
        {
            if (associatedDll == null) throw new ArgumentNullException("associatedDll");
            _associatedDll = associatedDll;
        }


        public string Get()
        {
            var location = _associatedDll.FullPath;
            if (string.IsNullOrEmpty(location)) throw new FileNotFoundException(location);

            return Path.Combine(Path.GetDirectoryName(location) ?? string.Empty, Path.GetFileNameWithoutExtension(location) + ProgramConfigurationFileExtension);
        }
    }
}
