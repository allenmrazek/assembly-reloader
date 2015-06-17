using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Queries.FileSystemQueries;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IFilePathProvider _configurationFileProvider;
        private readonly IConfigNodeSerializer _configNodeSerializer;
        private readonly ILog _log;

        public ConfigurationProvider(
            [NotNull] IFilePathProvider configurationFileProvider, 
            [NotNull] IConfigNodeSerializer configNodeSerializer,
            [NotNull] ILog log)
        {
            if (configurationFileProvider == null) throw new ArgumentNullException("configurationFileProvider");
            if (configNodeSerializer == null) throw new ArgumentNullException("configNodeSerializer");
            if (log == null) throw new ArgumentNullException("log");
            _configurationFileProvider = configurationFileProvider;
            _configNodeSerializer = configNodeSerializer;
            _log = log;
        }


        public Configuration Get()
        {
            var configuration = new Configuration();
            var configPath = _configurationFileProvider.Get();
            var diskConfig = LoadConfigNode(configPath);

            if (!File.Exists(configPath))
            {
                _log.Warning("No file found at " + configPath + "; will use default configuration settings");
                return configuration;
            }


            if (diskConfig.Any())
                _configNodeSerializer.Deserialize(configuration, diskConfig.Single());
            else _log.Error("Failed to load ConfigNode at " + configPath);

            return configuration;
        }


        private static Maybe<ConfigNode> LoadConfigNode(string location)
        {
            if (!File.Exists(location)) return Maybe<ConfigNode>.None;

            var config = ConfigNode.Load(location);

            return config == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(config);
        }
    }
}
