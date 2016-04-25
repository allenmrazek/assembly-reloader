using System;
using System.IO;
using ReeperKSP.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once UnusedMember.Global
    public class GetConfigurationFilePath : IGetConfigurationFilePath
    {
        private readonly string _extension;

        public GetConfigurationFilePath() : this(".config")
        {
            
        }


        public GetConfigurationFilePath(string extension)
        {
            if (extension == null) throw new ArgumentNullException("extension");
            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException("cannot be null or empty", "extension");

            _extension = extension;
            if (!_extension.StartsWith(".", StringComparison.InvariantCulture)) _extension = "." + extension;
        }


        public string Get(IFile pluginLocation)
        {
            var pluginDir = pluginLocation.Directory;
            var filePath = Path.Combine(pluginDir.FullPath, pluginLocation.FileName + _extension);


            return filePath;
        }
    }
}
