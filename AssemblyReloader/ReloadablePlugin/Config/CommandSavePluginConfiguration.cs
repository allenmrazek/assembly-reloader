using System;
using AssemblyReloader.Gui;
using ReeperCommon.Logging;
using ReeperKSP.Extensions;
using ReeperKSP.Serialization;
using ReeperKSP.Serialization.Exceptions;
using strange.extensions.command.impl;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Config
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandSavePluginConfiguration : Command
    {
        private readonly IPluginInfo _plugin;
        private PluginConfiguration _pluginConfiguration;
        private readonly IConfigNodeSerializer _serializer;
        private readonly IGetConfigurationFilePath _configPath;
        private readonly SignalOnSaveConfiguration _onSaveSignal;
        private readonly ILog _log;

        public CommandSavePluginConfiguration(
            IPluginInfo plugin,
            PluginConfiguration pluginConfiguration,
            IConfigNodeSerializer serializer,
            IGetConfigurationFilePath configPath, 
            SignalOnSaveConfiguration onSaveSignal,
            ILog log)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (pluginConfiguration == null) throw new ArgumentNullException("pluginConfiguration");
            if (serializer == null) throw new ArgumentNullException("serializer");
            if (configPath == null) throw new ArgumentNullException("configPath");
            if (onSaveSignal == null) throw new ArgumentNullException("onSaveSignal");
            if (log == null) throw new ArgumentNullException("log");

            _plugin = plugin;
            _pluginConfiguration = pluginConfiguration;
            _serializer = serializer;
            _configPath = configPath;
            _onSaveSignal = onSaveSignal;
            _log = log;
        }

        public override void Execute()
        {
            _log.Verbose("Saving plugin configuration...");

            try
            {
                var cfg = new ConfigNode(PluginConfiguration.NodeName);
                _serializer.WriteObjectToConfigNode(ref _pluginConfiguration, cfg);

                _onSaveSignal.Dispatch(cfg);
                cfg.Write(_configPath.Get(_plugin.Location), _plugin.Name + " Configuration");

                _log.Normal("Plugin configuration saved");
            }
            catch (ReeperSerializationException re)
            {
                _log.Error("Exception while serializing data: " + re);
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "ReeperSerialization Error",
                    "Failed to serialize plugin configuration. See log.", "Okay", true, HighLogic.UISkin);
            }
            catch (Exception e)
            {
                _log.Error("General exception while saving plugin configuration! " + e);
                PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), "General Exception",
                    "Failed to save plugin configuration. See log.", "Okay", true, HighLogic.UISkin);
            }
        }
    }
}
