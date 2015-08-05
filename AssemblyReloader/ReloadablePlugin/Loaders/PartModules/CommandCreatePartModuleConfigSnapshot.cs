using System;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.Unsorted;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCreatePartModuleConfigNodeSnapshot : Command
    {
        private readonly MonoBehaviour _mbBeingDestroyed;
        private readonly IGetPartIsPrefab _partIsPrefabQuery;
        private readonly IPartModuleConfigNodeSnapshotRepository _snapshotRepository;
        private readonly IKspFactory _kspFactory;
        private readonly IGetUniqueFlightID _flightId;
        private readonly IGetTypeIdentifier _typeIdentifierQuery;
        private readonly ILog _log;


        public CommandCreatePartModuleConfigNodeSnapshot(
            MonoBehaviour mbBeingDestroyed,
            IGetPartIsPrefab partIsPrefabQuery,
            IPartModuleConfigNodeSnapshotRepository snapshotRepository,
            IKspFactory kspFactory,
            IGetUniqueFlightID flightId,
            IGetTypeIdentifier typeIdentifierQuery,
            ILog log)
        {
            if (mbBeingDestroyed == null) throw new ArgumentNullException("mbBeingDestroyed");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (snapshotRepository == null) throw new ArgumentNullException("snapshotRepository");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (flightId == null) throw new ArgumentNullException("flightId");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (log == null) throw new ArgumentNullException("log");

            _mbBeingDestroyed = mbBeingDestroyed;
            _partIsPrefabQuery = partIsPrefabQuery;
            _snapshotRepository = snapshotRepository;
            _kspFactory = kspFactory;
            _flightId = flightId;
            _typeIdentifierQuery = typeIdentifierQuery;
            _log = log;
        }


        public override void Execute()
        {
            if (!_mbBeingDestroyed.GetType().IsSubclassOf(typeof(PartModule)))
                return;

            var partModule = _mbBeingDestroyed as PartModule;

            if (partModule == null)
            {
                _log.Warning("Failed to cast target to PartModule");
                return;
            }

            var part = _kspFactory.Create(partModule.part);

            if (_partIsPrefabQuery.Get(part))
                return; // don't create snapshots for prefab PartModules; we'll use their default ConfigNodes when constructing them

            CreateSnapshot(part, partModule);
        }


        private void CreateSnapshot(IPart part, PartModule partModule)
        {
            // some parts (specifically those in the editor) don't have a flightID assigned by default. It's how
            // the game uniquely identifies parts so we might as well make use of it, too
            if (part.FlightID == 0)
            {
                _log.Debug("Non-prefab part does not have a flightID; assigning it a unique id");
                part.FlightID = _flightId.Get();
            }

            try
            {
                var config = new ConfigNode("MODULE");
                var identifier = _typeIdentifierQuery.Get(partModule.GetType());

                partModule.Save(config);
                _snapshotRepository.Store(part.FlightID, identifier, config);

                _log.Debug("Created snapshot of " + identifier + " on " + part.FlightID);
            }
            catch (Exception e)
            {
                _log.Error("Failed to create snapshot of " + partModule.moduleName + " on " + part.FlightID + ": " + e);
            }
        }
    }
}
