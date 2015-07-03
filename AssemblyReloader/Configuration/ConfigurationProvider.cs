using System;
using System.IO;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Configuration.Names;
using AssemblyReloader.FileSystem;
using AssemblyReloader.Properties;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Configuration
{
// ReSharper disable once UnusedMember.Global
// ReSharper disable once ClassNeverInstantiated.Global
    public class ConfigurationProvider : IConfigurationProvider
    {
        private readonly Assembly _coreAssembly;
        private readonly IFilePathProvider<Assembly> _configPathProvider;
        private readonly IConfigNodeSerializer _configNodeSerializer;
        private readonly ILog _log;

        public ConfigurationProvider(
            [NotNull, Name(AssemblyNames.Core)] Assembly coreAssembly,
            [NotNull] IFilePathProvider<Assembly> configPathProvider, 
            [NotNull] IConfigNodeSerializer configNodeSerializer,
            [NotNull] ILog log)
        {
            if (coreAssembly == null) throw new ArgumentNullException("coreAssembly");
            if (configPathProvider == null) throw new ArgumentNullException("configPathProvider");
            if (configNodeSerializer == null) throw new ArgumentNullException("configNodeSerializer");
            if (log == null) throw new ArgumentNullException("log");

            _coreAssembly = coreAssembly;
            _configPathProvider = configPathProvider;
            _configNodeSerializer = configNodeSerializer;
            _log = log;
        }


        public Configuration Get()
        {
            var configuration = new Configuration();
            var configPath = _configPathProvider.Get(_coreAssembly);
            var diskConfig = LoadConfigNode(configPath);

            if (!diskConfig.Any())
            {
                _log.Warning("No file found at " + configPath + "; will use default configuration settings");
                return configuration;
            }


            if (diskConfig.Any())
            {
                _log.Verbose("Deserializing Configuration found at \"" + configPath + "\"");
                _configNodeSerializer.Deserialize(configuration, diskConfig.Single());
            }
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
