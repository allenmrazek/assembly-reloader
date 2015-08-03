﻿using System;
using System.Linq;
using System.Reflection;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleFactory : IPartModuleFactory
    {
        private readonly IGetPartIsPrefab _isPartPrefabQuery;
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly IPartModuleConfigNodeSnapshotRepository _configNodeSnapshots;
        private readonly SignalPartModuleCreated _partModuleCreatedSignal;
        private readonly ILog _log;

        public PartModuleFactory(
            IGetPartIsPrefab isPartPrefabQuery,
            IPartModuleSettings partModuleSettings,
            IPartModuleConfigNodeSnapshotRepository configNodeSnapshots,
            SignalPartModuleCreated partModuleCreatedSignal,
            [Name(LogKeys.PartModuleFactory)] ILog log)
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

            _log.Debug("Creating PartModule " + descriptor.Identifier + " on " + part.FlightID);

            var result = part.GameObject.AddComponent(descriptor.Type) as PartModule;

            if (result == null)
                throw new Exception("Failed to add " + descriptor.Type.FullName + " to " + part.PartName);

            part.Modules.Add(result);


            // if this is the prefab GameObject, it will never become active again and awake will never
            // get called so we must do it ourselves
            if (_isPartPrefabQuery.Get(part))
            {
                var method = typeof (PartModule).GetMethod("Awake",
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

                if (method != null)
                    method.Invoke(result, null);
            }


            LoadConfigNodeFor(part, result, descriptor);
            _partModuleCreatedSignal.Dispatch(part, result, descriptor);
        }


        private void LoadConfigNodeFor(IPart part, PartModule partModule, PartModuleDescriptor descriptor)
        {
            if (_partModuleSettings.SaveAndReloadPartModuleConfigNodes && !_isPartPrefabQuery.Get(part))
            {
                // we might have a stored ConfigNode to be used for this PartModule
                if (_configNodeSnapshots.Peek(part.FlightID, descriptor.Identifier).Any())
                {
                    var config = _configNodeSnapshots.Retrieve(part.FlightID, descriptor.Identifier).Single();

                    try
                    {
                        partModule.Load(config);
                        _log.Verbose("Loaded stored ConfigNode for " + descriptor.Identifier + " on " + part.FlightID);
                    }
                    catch (Exception e)
                    {
                        LoadFailed("Encountered exception while loading snapshot ConfigNode", part, config, descriptor, e);
                    }
                    return;
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
            ConfigNode config, 
            PartModuleDescriptor descriptor, 
            Exception e)
        {
            _log.Warning(message + " on PartModule " + descriptor.Identifier + " on " + part.FlightID);
            _log.Warning("Tried to use ConfigNode: " + (config == null ? "<null>" : config.ToString()));
            _log.Warning("Exception was: " + e);
            _log.Warning("This ParTModule may have been partially initialized");
        }
    }
}
