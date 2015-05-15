using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Queries.FileSystemQueries;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Providers
{
    public class PluginConfigurationProvider : IPluginConfigurationProvider
    {
        private readonly IConfigNodeFormatter _configNodeFormatter;
        private readonly IPluginConfigurationFilePathQuery _configFilePathQuery;


        public PluginConfigurationProvider(
            [NotNull] IConfigNodeFormatter configNodeFormatter,
            [NotNull] IPluginConfigurationFilePathQuery configFilePathQuery)
        {
            if (configNodeFormatter == null) throw new ArgumentNullException("configNodeFormatter");
            if (configFilePathQuery == null) throw new ArgumentNullException("configFilePathQuery");

            _configNodeFormatter = configNodeFormatter;
            _configFilePathQuery = configFilePathQuery;
        }


        public PluginConfiguration Get([NotNull] IFile pluginLocation)
        {
            if (pluginLocation == null) throw new ArgumentNullException("pluginLocation");

            var config = new PluginConfiguration();

            var configPath = _configFilePathQuery.Get(pluginLocation);

            if (File.Exists(configPath)) DeserializeConfig(config, configPath);

            return config;
        }


        private void DeserializeConfig(PluginConfiguration config, string configNodeLocation)
        {
            var node = LoadConfigNode(configNodeLocation);
            if (!node.Any())
                throw new FileNotFoundException("Did not find a ConfigNode definition at " + configNodeLocation);

            _configNodeFormatter.Deserialize(config, node.Single());
        }


        private Maybe<ConfigNode> LoadConfigNode(string location)
        {
            var config = ConfigNode.Load(location);

            return config == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(config);
        }
    }
}
