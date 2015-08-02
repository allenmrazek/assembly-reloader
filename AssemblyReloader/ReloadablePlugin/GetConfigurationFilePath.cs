using System;
using System.IO;
using AssemblyReloader.Properties;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once UnusedMember.Global
    public class GetConfigurationFilePath : IGetConfigurationFilePath
    {
        private readonly string _extension;

        public GetConfigurationFilePath([NotNull] string extension = ".config")
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
