extern alias KSP;
using System;
using System.Linq;
using System.Reflection;
using ReeperCommon.Logging;
using strange.extensions.implicitBind;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleFactory : IPartModuleFactory
    {
        private const string ActionNodeName = "ACTIONS";
        private const string EventNodeName = "EVENTS";

        private readonly IQueryPartIsPrefab _isPartPrefabQuery;
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly IPartModuleConfigNodeSnapshotRepository _configNodeSnapshots;
        private readonly SignalPartModuleCreated _partModuleCreatedSignal;
        private readonly ILog _log;


        public PartModuleFactory(
            IQueryPartIsPrefab isPartPrefabQuery,
            IPartModuleSettings partModuleSettings,
            IPartModuleConfigNodeSnapshotRepository configNodeSnapshots,
            SignalPartModuleCreated partModuleCreatedSignal,
            [Name(LogKey.PartModuleFactory)] ILog log)
        {
            if (isPartPrefabQuery == null) throw new ArgumentNullException("isPartPrefabQuery");
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (configNodeSnapshots == null) throw new ArgumentNullException("configNodeSnapshots");
            if (partModuleCreatedSignal == null) throw new ArgumentNullException("partModuleCreatedSignal");
            if (log == null) throw new ArgumentNullException("log");

            _isPartPrefabQuery = isPartPrefabQuery;
            _partModuleSettings = partModuleSettings;
            _configNodeSnapshots = configNodeSnapshots;
            _partModuleCreatedSignal = partModuleCreatedSignal;
            _log = log;
        }


        public void Create(IPart part, PartModuleDescriptor descriptor)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            // todo: update ProtoPartSnapshots

            _log.Debug("Creating PartModule " + descriptor.Identifier + " on " + part.FlightID);

            var result = part.GameObject.AddComponent(descriptor.Type) as KSP::PartModule;

            if (result == null)
                throw new Exception("Failed to add " + descriptor.Type.FullName + " to " + part.PartName);

            part.Modules.Add(result);


            // if this is the prefab GameObject, it will never become active again and awake will never
            // get called so we must do it ourselves
            if (_isPartPrefabQuery.Get(part))
            {
                var method = typeof (KSP::PartModule).GetMethod("Awake",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                    method.Invoke(result, null);
            }


            LoadConfigNodeFor(part, result, descriptor);
            _partModuleCreatedSignal.Dispatch(part, result, descriptor);
        }


        // todo: clean this up, zomg cyclomatic complexity batman
        private void LoadConfigNodeFor(IPart part, KSP::PartModule partModule, PartModuleDescriptor descriptor)
        {
            if (_partModuleSettings.SaveAndReloadPartModuleConfigNodes && !_isPartPrefabQuery.Get(part))
            {
                // we might have a stored ConfigNode to be used for this PartModule
                if (_configNodeSnapshots.Peek(part.FlightID, descriptor.Identifier).Any())
                {
                    var config = _configNodeSnapshots.Retrieve(part.FlightID, descriptor.Identifier).Single();

                    if (_partModuleSettings.ResetPartModuleActions)
                    {
                        if (config.HasNode(ActionNodeName))
                        {
                            _log.Verbose("Removing PartModule ACTIONS node from snapshot");
                            config.RemoveNode(ActionNodeName);
                        }
                        else _log.Warning("Did not find ACTIONS node on snapshot");
                    }

                    if (_partModuleSettings.ResetPartModuleEvents)
                    {
                        if (config.HasNode(EventNodeName))
                        {
                            _log.Verbose("Removing PartModule EVENTS node from snapshot");
                            config.RemoveNode(EventNodeName);
                        }
                        else _log.Warning("Did not find EVENTS node on snapshot");
                    }

                    _log.Debug("Using ConfigNode: {0}", config.ToString());

                    try
                    {
                        partModule.Load(config);
                        _log.Verbose("Loaded stored ConfigNode for " + descriptor.Identifier + " on " + part.FlightID);
                        return;
                    }
                    catch (Exception e)
                    {
                        LoadFailed("Encountered exception while loading snapshot ConfigNode", part, config, descriptor, e);
                    }
                }
            }

            try
            {
                partModule.Load(descriptor.Config);
                _log.Verbose("Loaded default ConfigNode for " + descriptor.Identifier + " on " + part.FlightID);
            }
            catch (Exception e)
            {
                LoadFailed("Encountered exception while loading default ConfigNode", part, descriptor.Config, descriptor, e);
            }
            
        }


        private void LoadFailed(
            string message, 
            IPart part, 
            KSP::ConfigNode config, 
            PartModuleDescriptor descriptor, 
            Exception e)
        {
            _log.Warning(message + " on PartModule " + descriptor.Identifier + " on " + part.FlightID);
            _log.Warning("Tried to use ConfigNode: " + (config == null ? "<null>" : config.ToString()));
            _log.Warning("Exception was: " + e);
            _log.Warning("This PartModule may have been partially initialized");
        }
    }
}
