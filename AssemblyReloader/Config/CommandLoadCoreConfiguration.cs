extern alias KSP;
extern alias Cecil96;
using System;
using System.IO;
using AssemblyReloader.ReloadablePlugin;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;
using strange.extensions.command.impl;
using Cecil96::Mono.Cecil;
using HighLogic = KSP::HighLogic;

namespace AssemblyReloader.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadCoreConfiguration : Command
    {
        private readonly CoreConfiguration _coreConfiguration;
        private readonly SignalOnLoadConfiguration _loadConfigSignal;
        private readonly IGetConfigurationFilePath _configPath;
        private readonly IConfigNodeSerializer _serializer;
        private readonly IFile _core;
        private readonly ILog _log;

        public CommandLoadCoreConfiguration(
            CoreConfiguration coreConfiguration,
            SignalOnLoadConfiguration loadConfigSignal,
            IGetConfigurationFilePath configPath,
            IConfigNodeSerializer serializer,
            IFile core,
            ILog log)
        {
            if (coreConfiguration == null) throw new ArgumentNullException("coreConfiguration");
            if (loadConfigSignal == null) throw new ArgumentNullException("loadConfigSignal");
            if (configPath == null) throw new ArgumentNullException("configPath");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (core == null) throw new ArgumentNullException("core");
            if (log == null) throw new ArgumentNullException("log");

            _coreConfiguration = coreConfiguration;
            _loadConfigSignal = loadConfigSignal;
            _configPath = configPath;
            _serializer = serializer;
            _core = core;
            _log = log;
        }

        public override void Execute()
        {
            _log.Debug("Mono.Cecil: " + typeof(AssemblyDefinition).Assembly.FullName);

            var fullPath = _configPath.Get(_core);

            if (!File.Exists(fullPath))
            {
                _log.Warning("No configuration path found at \"" + fullPath + "\"; using default configuration settings");
                return;
            }

            var config = KSP::ConfigNode.Load(fullPath);
            if (config == null || !config.HasData || (config = config.GetNode(CoreConfiguration.NodeName)) == null)
            {
                _log.Error("Failed to load ConfigNode at " + fullPath);
                return;
            }

            try
            {
                _log.Normal("Loading configuration...");
                _serializer.Deserialize(_coreConfiguration, config);
                _loadConfigSignal.Dispatch(config);
                _log.Normal("Configuration loaded successfully.");
            }
            catch (ReeperSerializationException rse)
            {
                _log.Error("Error while deserializing core configuration: object might be in inconsistent state. " + rse);
                KSP::PopupDialog.SpawnPopupDialog("Core Configuration Exception",
                    "The core configuration wasn't deserialized correctly.", "Accept", true, HighLogic.Skin);
            }
        }
    }
}
