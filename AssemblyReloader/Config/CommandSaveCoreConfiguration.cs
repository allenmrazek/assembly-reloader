extern alias KSP;
using System.IO;
using System.Reflection;
using ReeperCommon.Extensions;
using ReeperCommon.Serialization;
using ReeperCommon.Serialization.Exceptions;
using ConfigNode = KSP::ConfigNode;
using System;
using AssemblyReloader.ReloadablePlugin;
using ReeperCommon.FileSystem;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.Config
{
// ReSharper disable once UnusedMember.Global
    public class CommandSaveCoreConfiguration : Command
    {
        private static readonly string ConfigurationFileHeader = Assembly.GetExecutingAssembly().GetName().Name + " v" + Assembly.GetExecutingAssembly().GetName().Version;

        private CoreConfiguration _coreConfiguration;
        private readonly SignalOnSaveConfiguration _saveConfigSignal;
        private readonly IGetConfigurationFilePath _configPath;
        private readonly IConfigNodeSerializer _serializer;
        private readonly IFile _core;
        private readonly ILog _log;

        public CommandSaveCoreConfiguration(
            CoreConfiguration coreConfiguration,
            SignalOnSaveConfiguration saveConfigSignal,
            IGetConfigurationFilePath configPath,
            IConfigNodeSerializer serializer,
            IFile core,
            ILog log)
        {
            if (coreConfiguration == null) throw new ArgumentNullException("coreConfiguration");
            if (saveConfigSignal == null) throw new ArgumentNullException("saveConfigSignal");
            if (configPath == null) throw new ArgumentNullException("configPath");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (core == null) throw new ArgumentNullException("core");
            if (log == null) throw new ArgumentNullException("log");

            _coreConfiguration = coreConfiguration;
            _saveConfigSignal = saveConfigSignal;
            _configPath = configPath;
            _serializer = serializer;
            _core = core;
            _log = log;
        }


        public override void Execute()
        {
            _log.Normal("Saving configuration...");

            var cfg = new ConfigNode(CoreConfiguration.NodeName);

            try
            {
                try
                {
                    _serializer.WriteObjectToConfigNode(ref _coreConfiguration, cfg);
                }
                catch (ReeperSerializationException rse)
                {
                    _log.Error("Exception while serializing core configuration: " + rse);
                }

                _saveConfigSignal.Dispatch(cfg);

                cfg.Write(_configPath.Get(_core), ConfigurationFileHeader);

                _log.Normal("Configuration saved");
                _log.Debug("Configuration: {0}", cfg.ToString());
            }
            catch (ReeperSerializationException re)
            {
                _log.Error("Serialization exception occurred while saving core configuration! " + re);
            }
            catch (IOException ioe)
            {
                _log.Error("IO exception occurred while saving core configuration: " + ioe);
            }
        }
    }
}
