using System.IO;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Queries.FileSystemQueries
{
    public class ConfigurationFilePathQuery : IConfigurationFilePathQuery
    {
        private const string PluginConfigFilename = "ArtPluginConfiguration.cfg";


        public Maybe<string> Get(IFile pluginLocation)
        {
            var pluginDir = pluginLocation.Directory;
            var filePath = Path.Combine(pluginDir.FullPath, PluginConfigFilename);

            if (!Directory.Exists(pluginDir.FullPath)) return Maybe<string>.None;

            return File.Exists(filePath) ? Maybe<string>.With(filePath) : Maybe<string>.None;
        }
    }
}
