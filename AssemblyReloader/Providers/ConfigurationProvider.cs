using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Queries.FileSystemQueries;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Providers
{
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly IFile _assemblyLocation;
        private readonly IPluginConfigurationFilePathQuery _configFilePathQuery;
        private readonly ILog _log;

        public ConfigurationProvider(
            [NotNull] IFile assemblyLocation,
            [NotNull] IPluginConfigurationFilePathQuery configFilePathQuery, 
            [NotNull] ILog log)
        {
            if (assemblyLocation == null) throw new ArgumentNullException("assemblyLocation");
            if (configFilePathQuery == null) throw new ArgumentNullException("configFilePathQuery");
            if (log == null) throw new ArgumentNullException("log");
            _assemblyLocation = assemblyLocation;
            _configFilePathQuery = configFilePathQuery;
            _log = log;
        }


        public Configuration Get()
        {
            var config = new Configuration();
            var configPath = _configFilePathQuery.Get(_assemblyLocation);

            if (!File.Exists(configPath))
            {
                _log.Warning("No file found at " + configPath + "; will use default settings");
                return config;
            }

            var node = LoadConfigNode(configPath);

            if (!node.Any())
                throw new Exception("Couldn't load ConfigNode from " + configPath);

            if (!ConfigNode.LoadObjectFromConfig(config, node.Single()))
                throw new Exception("Failed to load Configuration from ConfigNode");

            return config;
        }


        private static Maybe<ConfigNode> LoadConfigNode(string location)
        {
            var config = ConfigNode.Load(location);

            return config == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(config);
        }
    }
}
