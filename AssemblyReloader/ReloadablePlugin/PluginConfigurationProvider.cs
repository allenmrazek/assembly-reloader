using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Config;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin.Config;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.ReloadablePlugin
{
    [Implements(typeof(IPluginConfigurationProvider))]
// ReSharper disable once ClassNeverInstantiated.Global
    public class PluginConfigurationProvider : IPluginConfigurationProvider
    {
        private readonly IGetConfigurationFilePath _configFilePath;


        public PluginConfigurationProvider(
            [NotNull] IGetConfigurationFilePath configFilePath)
        {
            if (configFilePath == null) throw new ArgumentNullException("configFilePath");

            _configFilePath = configFilePath;
        }


        public PluginConfiguration Get([NotNull] IFile pluginLocation)
        {
            if (pluginLocation == null) throw new ArgumentNullException("pluginLocation");

            var config = new PluginConfiguration();

            var configPath = _configFilePath.Get(pluginLocation);

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
