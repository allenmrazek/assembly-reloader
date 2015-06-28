using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Queries.FileSystemQueries;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PluginConfigurationProvider : IPluginConfigurationProvider
    {
        private readonly IPluginConfigurationFilePathQuery _configFilePathQuery;


        public PluginConfigurationProvider(
            [NotNull] IPluginConfigurationFilePathQuery configFilePathQuery)
        {
            if (configFilePathQuery == null) throw new ArgumentNullException("configFilePathQuery");

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


        private static void DeserializeConfig(PluginConfiguration config, string configNodeLocation)
        {
            var node = LoadConfigNode(configNodeLocation);
            if (!node.Any())
                throw new FileNotFoundException("Did not find a ConfigNode definition at " + configNodeLocation);

            if (!ConfigNode.LoadObjectFromConfig(config, node.Single()))
                throw new Exception("Failed to load PluginConfiguration from ConfigNode");
        }


        private static Maybe<ConfigNode> LoadConfigNode(string location)
        {
            var config = ConfigNode.Load(location);

            return config == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(config);
        }
    }
}
