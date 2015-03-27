using System;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Generators;
using AssemblyReloader.Queries;
using AssemblyReloader.Repositories;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public class PartModuleSnapshotGenerator : IPartModuleSnapshotGenerator
    {
        private readonly IFlightConfigRepository _repository;
        private readonly IPartIsPrefabQuery _partIsPrefabQuery;
        private readonly ITypeIdentifierQuery _typeIdentifierQuery;
        private readonly IUniqueFlightIdGenerator _flightIdGenerator;
        private readonly ILog _log;

        public PartModuleSnapshotGenerator(
            [NotNull] IFlightConfigRepository repository,
            [NotNull] IPartIsPrefabQuery partIsPrefabQuery,
            [NotNull] ITypeIdentifierQuery typeIdentifierQuery, 
            [NotNull] IUniqueFlightIdGenerator flightIdGenerator,
            [NotNull] ILog log)
        {
            if (repository == null) throw new ArgumentNullException("repository");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (flightIdGenerator == null) throw new ArgumentNullException("flightIdGenerator");
            if (log == null) throw new ArgumentNullException("log");

            _repository = repository;
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

                _repository.Store(part.FlightID, _typeIdentifierQuery.Get(instance.GetType()), node);
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
