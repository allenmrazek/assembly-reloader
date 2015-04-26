using System.IO;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class ConfigurationFilePathQuery : IConfigurationFilePathQuery
    {
        public string Get(IFile pluginLocation)
        {
            var pluginDir = pluginLocation.Directory;
            var filePath = Path.Combine(pluginDir.FullPath, pluginLocation.FileName + ".config");


            return filePath;
        }
    }
}
