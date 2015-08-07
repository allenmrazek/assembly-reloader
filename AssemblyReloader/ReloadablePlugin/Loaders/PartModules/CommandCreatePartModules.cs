using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCreatePartModules : Command
    {
        private readonly IPartModuleLoader _partModuleLoader;
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly IPartModuleConfigNodeSnapshotRepository _snapshotRepository;
        private readonly SignalLoadersFinished _loadersFinishedSignal;
        private readonly SignalPartModuleCreated _createdSignal;
        private readonly IGetPartModuleStartState _startStateProvider;
        private readonly IGetPartIsPrefab _partIsPrefabQuery;
        private readonly ILog _log;

        private readonly List<KeyValuePair<IPart, PartModule>> _targets = new List<KeyValuePair<IPart, PartModule>>();


        public CommandCreatePartModules(
            IPartModuleLoader partModuleLoader, 
            ILoadedAssemblyHandle handle,
            IPartModuleSettings partModuleSettings,
            IPartModuleConfigNodeSnapshotRepository snapshotRepository,
            SignalLoadersFinished loadersFinishedSignal,
            SignalPartModuleCreated createdSignal,
            IGetPartModuleStartState startStateProvider,
            IGetPartIsPrefab partIsPrefabQuery,
            ILog log)
        {
            if (partModuleLoader == null) throw new ArgumentNullException("partModuleLoader");
            if (handle == null) throw new ArgumentNullException("handle");
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (snapshotRepository == null) throw new ArgumentNullException("snapshotRepository");
            if (loadersFinishedSignal == null) throw new ArgumentNullException("loadersFinishedSignal");
            if (createdSignal == null) throw new ArgumentNullException("createdSignal");
            if (startStateProvider == null) throw new ArgumentNullException("startStateProvider");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleLoader = partModuleLoader;
            _handle = handle;
            _partModuleSettings = partModuleSettings;
            _snapshotRepository = snapshotRepository;
            _loadersFinishedSignal = loadersFinishedSignal;
            _createdSignal = createdSignal;
            _startStateProvider = startStateProvider;
            _partIsPrefabQuery = partIsPrefabQuery;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Creating PartModules");

            _createdSignal.AddListener(OnPartModuleCreated);
            _partModuleLoader.Load(_handle, !_partModuleSettings.ReloadPartModuleInstancesImmediately);
            _createdSignal.RemoveListener(OnPartModuleCreated);

            _loadersFinishedSignal.AddListener(OnLoadersFinished);
            Retain();
        }


        private void OnPartModuleCreated(IPart part, PartModule pm, PartModuleDescriptor descriptor)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (pm == null) throw new ArgumentNullException("pm");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            if (_partIsPrefabQuery.Get(part))
                return;

            _targets.Add(new KeyValuePair<IPart, PartModule>(part, pm));
        }


        private void OnLoadersFinished()
        {
            _loadersFinishedSignal.RemoveListener(OnLoadersFinished);
            _snapshotRepository.Clear();

            Release();
            RunOnStarts();
        }


        private void RunOnStarts()
        {
            // note to self: it's possible for parts not to be associated with any vessel,
            // such as when constructing a ship in the editor
            var defaultGuid = Guid.NewGuid();

            var groupedByVessel = _targets
                .GroupBy(target => target.Key.Vessel.Any() ? target.Key.Vessel.Single().ID : defaultGuid, target => target)
                .ToList();

            foreach (var group in groupedByVessel)
            {
                var partModulesInGroup = group.ToList();

                if (!partModulesInGroup.Any())
                {
                    _log.Debug("No PartModules in grouping; skipping");
                    continue;
                }

                RunOnStartsForVessel(partModulesInGroup.First().Key.Vessel, partModulesInGroup);
            }
        }


        private void RunOnStartsForVessel(Maybe<IVessel> vessel, IEnumerable<KeyValuePair<IPart, PartModule>> targets)
        {
            var state = _startStateProvider.Get(vessel);

            // group target partModules by part
            foreach (var target in targets.OrderBy(t => t.Key.FlightID))
            {
                try
                {
                    _log.Debug("Running OnStart for " + target.Value.moduleName + " on " + target.Key.FlightID);
                    target.Value.OnStart(state);
                }
                catch (Exception e)
                {
                    _log.Warning("Exception while running OnStart for " + target.Value.moduleName + " on " +
                                 target.Key.FlightID + ": " + e);
                }
            }
        }
    }
}
