using System;
using System.IO;
using System.Linq;
using AssemblyReloader.Gui;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using ReeperKSP.Serialization;
using ReeperKSP.Serialization.Exceptions;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadPluginConfiguration : Command
    {
        private readonly IPluginInfo _plugin;
        private PluginConfiguration _configuration;
        private readonly IConfigNodeSerializer _serializer;
        private readonly IGetConfigurationFilePath _configPath;
        private readonly SignalOnLoadConfiguration _loadConfigurationSignal;
        private readonly ILog _log;

        public CommandLoadPluginConfiguration(
            IPluginInfo plugin,
            PluginConfiguration configuration, 
            IConfigNodeSerializer serializer,
            IGetConfigurationFilePath configPath,
            SignalOnLoadConfiguration loadConfigurationSignal,
            ILog log)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (configPath == null) throw new ArgumentNullException("configPath");
            if (loadConfigurationSignal == null) throw new ArgumentNullException("loadConfigurationSignal");
            if (log == null) throw new ArgumentNullException("log");

            _plugin = plugin;
            _configuration = configuration;
            _serializer = serializer;
            _configPath = configPath;
            _loadConfigurationSignal = loadConfigurationSignal;
            _log = log;
        }


        public override void Execute()
        {
            var configFullpath = _configPath.Get(_plugin.Location);

            if (!File.Exists(_configPath.Get(_plugin.Location)))
            {
                _log.Normal("No configuration file found at \"" + configFullpath + "\"; defaults will be used");
                return;
            }

            try
            {
                _log.Normal("Loading configuration");

                var config = ConfigNode.Load(configFullpath)
                    .With(c => c.GetNode(PluginConfiguration.NodeName))
                    .ToMaybe();

                if (!config.Any())
                {
                    _log.Error("Failed to read configuration file");
                    return;
                }

                _serializer.LoadObjectFromConfigNode(ref _configuration, config.Single());

                _loadConfigurationSignal.Dispatch(config.Single());

                _log.Normal("Configuration loaded successfully");
            }
            catch (ReeperSerializationException rse)
            {
                _log.Error("Serialization exception: " + rse);
            }
        }
    }
}
