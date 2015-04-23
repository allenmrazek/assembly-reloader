using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;
using ReeperCommon.FileSystem.Implementations;

namespace AssemblyReloader.Providers
{
    public class PluginConfigurationProvider : IPluginConfigurationProvider
    {
        private const string PluginConfigFilename = "ArtPluginConfiguration.cfg";
        private readonly IConfigNodeFormatterProvider _formatterProvider;


        public PluginConfigurationProvider([NotNull] IConfigNodeFormatterProvider formatterProvider)
        {
            if (formatterProvider == null) throw new ArgumentNullException("formatterProvider");

            _formatterProvider = formatterProvider;
        }


        public Configuration Get([NotNull] IFile pluginLocation)
        {
            if (pluginLocation == null) throw new ArgumentNullException("pluginLocation");

            var config = new Configuration();

            var pluginDir = pluginLocation.Directory;

            var configFile = pluginDir.File(new KSPUrlIdentifier(Path.Combine(pluginLocation.Url, PluginConfigFilename)));

            if (configFile.Any()) DeserializeConfig(config, configFile.FirstOrDefault());

            return config;
        }


        private void DeserializeConfig(Configuration config, IFile configNodeLocation)
        {
            var node = FindConfigNode(configNodeLocation);
            if (!node.Any())
                throw new FileNotFoundException("Did not find a ConfigNode definition at " + configNodeLocation.FileName);

            var formatter = _formatterProvider.Get();

            formatter.Deserialize(config, node.Single());
        }


        private Maybe<ConfigNode> FindConfigNode(IFile location)
        {
            var node = GameDatabase.Instance.GetConfigNode(location.Url);

            return node == null ? Maybe<ConfigNode>.None : Maybe<ConfigNode>.With(node);
        }
    }
}
