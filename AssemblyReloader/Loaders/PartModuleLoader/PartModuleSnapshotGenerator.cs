using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public class PartModuleSnapshotGenerator : IPartModuleSnapshotGenerator
    {
        private readonly DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> _configNodeQueue;
        private readonly IPartIsPrefabQuery _partIsPrefabQuery;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;
        private readonly IUniqueFlightIdGenerator _flightIdGenerator;
        private readonly ILog _log;

        public PartModuleSnapshotGenerator(
            [NotNull] DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> configNodeQueue,
            [NotNull] IPartIsPrefabQuery partIsPrefabQuery,
            [NotNull] ITypeIdentifierQuery typeIdentifierQuery, 
            [NotNull] IUniqueFlightIdGenerator flightIdGenerator,
            [NotNull] ILog log)
        {
            if (configNodeQueue == null) throw new ArgumentNullException("configNodeQueue");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (flightIdGenerator == null) throw new ArgumentNullException("flightIdGenerator");
            if (log == null) throw new ArgumentNullException("log");

            _configNodeQueue = configNodeQueue;
            _partIsPrefabQuery = partIsPrefabQuery;
            _typeIdentifierQuery = typeIdentifierQuery;
            _flightIdGenerator = flightIdGenerator;
            _log = log;
        }


        public void Snapshot(IPart part, PartModule instance)
        {
            if (_partIsPrefabQuery.Get(part))
                return; // no need to snapshot prefabs

            try
            {
                var node = new ConfigNode();

                instance.Save(node);

                if (part.FlightID == 0)
                    part.FlightID = _flightIdGenerator.Get();

                _configNodeQueue.Store(
                    new KeyValuePair<uint, ITypeIdentifier>(part.FlightID, _typeIdentifierQuery.Get(instance.GetType())),
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
