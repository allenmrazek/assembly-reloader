using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class CommandLoadPartModuleConfigNode : Command
    {
        private readonly IPart _part;
        private readonly PartModule _partModule;
        private readonly PartModuleDescriptor _descriptor;
        private readonly IGetPartIsPrefab _partIsPrefabQuery;
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly IPartModuleConfigNodeSnapshotRepository _configNodeSnapshots;
        private readonly ILog _log;


        public CommandLoadPartModuleConfigNode(
            IPart part,
            PartModule partModule, 
            PartModuleDescriptor descriptor,
            IGetPartIsPrefab partIsPrefabQuery,
            IPartModuleSettings partModuleSettings,
            IPartModuleConfigNodeSnapshotRepository configNodeSnapshots,
            [Name(LogKeys.PartModuleLoader)] ILog log)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (partModule == null) throw new ArgumentNullException("partModule");
            if (descriptor == null) throw new ArgumentNullException("descriptor");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (configNodeSnapshots == null) throw new ArgumentNullException("configNodeSnapshots");
            if (log == null) throw new ArgumentNullException("log");

            _part = part;
            _partModule = partModule;
            _descriptor = descriptor;
            _partIsPrefabQuery = partIsPrefabQuery;
            _partModuleSettings = partModuleSettings;
            _configNodeSnapshots = configNodeSnapshots;
            _log = log;
        }


        public override void Execute()
        {
            if (_partModuleSettings.SaveAndReloadPartModuleConfigNodes && !_partIsPrefabQuery.Get(_part))
            {
                // we might have a stored ConfigNode to be used for this PartModule
                if (_configNodeSnapshots.Peek(_part.FlightID, _descriptor.Identifier).Any())
                {
                    var config = _configNodeSnapshots.Retrieve(_part.FlightID, _descriptor.Identifier).Single();

                    try
                    {
                        _partModule.Load(config);
                        _log.Verbose("Loaded stored ConfigNode for " + _descriptor.Identifier + " on " + _part.FlightID);
                    }
                    catch (Exception e)
                    {
                        LoadFailed("Encountered exception while loading snapshot ConfigNode", config, e);
                    }
                    return;
                }
            }

            try
            {
                _partModule.Load(_descriptor.Config);
                _log.Verbose("Loaded default ConfigNode for " + _descriptor.Identifier + " on " + _part.FlightID);
            }
            catch (Exception e)
            {
                LoadFailed("Encountered exception while loading default ConfigNode", _descriptor.Config, e);
            }
            
        }


        private void LoadFailed(string message, ConfigNode config, Exception e)
        {
            _log.Warning(message + " on PartModule " + _descriptor.Identifier + " on " + _part.FlightID);
            _log.Warning("Tried to use ConfigNode: " + (config == null ? "<null>" : config.ToString()));
            _log.Warning("Exception was: " + e);
            _log.Warning("This ParTModule may have been partially initialized");
        }
    }
}
