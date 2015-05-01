using System;
using System.IO;
using AssemblyReloader.Annotations;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class ConfigurationFilePathQuery : IConfigurationFilePathQuery
    {
        private readonly string _extension;

        public ConfigurationFilePathQuery([NotNull] string extension = ".config")
        {
            if (extension == null) throw new ArgumentNullException("extension");
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException("extension");

            _extension = extension;
            if (!_extension.StartsWith(".")) _extension = "." + extension;
        }


        public string Get(IFile pluginLocation)
        {
            var pluginDir = pluginLocation.Directory;
            var filePath = Path.Combine(pluginDir.FullPath, pluginLocation.FileName + _extension);


            return filePath;
        }
    }
}
