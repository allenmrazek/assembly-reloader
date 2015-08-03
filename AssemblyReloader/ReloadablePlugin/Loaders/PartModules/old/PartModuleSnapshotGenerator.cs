﻿using System;
using System.Collections.Generic;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Properties;
using AssemblyReloader.Unsorted;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules.old
{
    public class PartModuleSnapshotGenerator : IPartModuleSnapshotGenerator
    {
        private readonly DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> _configNodeQueue;
        private readonly IGetPartIsPrefab _getIsPartPrefab;
        private readonly IGetTypeIdentifier _getTypeIdentifier;
        private readonly IGetUniqueFlightID _flightId;
        private readonly ILog _log;

        public PartModuleSnapshotGenerator(
            [NotNull] DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> configNodeQueue,
            [NotNull] IGetPartIsPrefab getIsPartPrefab,
            [NotNull] IGetTypeIdentifier getTypeIdentifier, 
            [NotNull] IGetUniqueFlightID flightId,
            [NotNull] ILog log)
        {
            if (configNodeQueue == null) throw new ArgumentNullException("configNodeQueue");
            if (getIsPartPrefab == null) throw new ArgumentNullException("getIsPartPrefab");
            if (getTypeIdentifier == null) throw new ArgumentNullException("getTypeIdentifier");
            if (flightId == null) throw new ArgumentNullException("flightId");
            if (log == null) throw new ArgumentNullException("log");

            _configNodeQueue = configNodeQueue;
            _getIsPartPrefab = getIsPartPrefab;
            _getTypeIdentifier = getTypeIdentifier;
            _flightId = flightId;
            _log = log;
        }


        public void Snapshot(IPart part, PartModule instance)
        {
            if (_getIsPartPrefab.Get(part))
                return; // no need to snapshot prefabs

            try
            {
                var node = new ConfigNode();

                instance.Save(node);

                if (part.FlightID == 0)
                    part.FlightID = _flightId.Get();

                _configNodeQueue.Store(
                    new KeyValuePair<uint, ITypeIdentifier>(part.FlightID, _getTypeIdentifier.Get(instance.GetType())),
                    node);

                _log.Debug("Created ConfigNode snapshot for part " + part.FlightID);
            }
            catch (Exception e)
            {
                _log.Error("The following exception occurred while generating a ConfigNode snapshot for " + part.Name +
                           " on part " + part.FlightID);
                _log.Error(e.ToString());

                _log.Warning(
                    "The config-supplied ConfigNode will be used for this PartModule instead of the previous version's snapshot.");
            }
        }
    }
}
