using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.command.impl;


namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadPartModules : Command
    {
        private readonly IPartModuleLoader _partModuleLoader;
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IPartModuleConfigNodeSnapshotRepository _snapshotRepository;
        private readonly SignalPartModuleCreated _createdSignal;
        private readonly IGetPartModuleStartState _startStateProvider;
        private readonly IQueryPartIsPrefab _partIsPrefabQuery;
        private readonly SignalLoadersFinished _loadersFinished;
        private readonly ILog _log;

        private readonly List<KeyValuePair<IPart, PartModule>> _targets = new List<KeyValuePair<IPart, PartModule>>();


        public CommandLoadPartModules(
            IPartModuleLoader partModuleLoader, 
            ILoadedAssemblyHandle handle,
            IPartModuleConfigNodeSnapshotRepository snapshotRepository,
            SignalPartModuleCreated createdSignal,
            IGetPartModuleStartState startStateProvider,
            IQueryPartIsPrefab partIsPrefabQuery,
            SignalLoadersFinished loadersFinished,
            ILog log)
        {
            if (partModuleLoader == null) throw new ArgumentNullException("partModuleLoader");
            if (handle == null) throw new ArgumentNullException("handle");
            if (snapshotRepository == null) throw new ArgumentNullException("snapshotRepository");
            if (createdSignal == null) throw new ArgumentNullException("createdSignal");
            if (startStateProvider == null) throw new ArgumentNullException("startStateProvider");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (loadersFinished == null) throw new ArgumentNullException("loadersFinished");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleLoader = partModuleLoader;
            _handle = handle;
            _snapshotRepository = snapshotRepository;
            _createdSignal = createdSignal;
            _startStateProvider = startStateProvider;
            _partIsPrefabQuery = partIsPrefabQuery;
            _loadersFinished = loadersFinished;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Creating PartModules");

            _createdSignal.AddListener(OnPartModuleCreated);
            _partModuleLoader.Load(_handle);
            _createdSignal.RemoveListener(OnPartModuleCreated);

            _snapshotRepository.Clear();

            _loadersFinished.AddOnce(InitializeModules);
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


        private void InitializeModules()
        {
            try
            {
                RunOnInitialize();
                RunOnStarts();
                Release();
            }
            catch (Exception e)
            {
                Release();
                throw;
            }
        }


        // note to self: these are NOT run for prefab parts
        private void RunOnInitialize()
        {
            var toBeInitialized = _targets.Where(kvp => !_partIsPrefabQuery.Get(kvp.Key)).ToList();

            if (!toBeInitialized.Any())
                return;

            _log.Verbose("Running PartModule OnInitialize for " + toBeInitialized.Count + " targets");

            foreach (var pm in toBeInitialized)
            {
                try // never trust the user ;)
                {
                    pm.Value.OnInitialize();
                }
                catch (Exception e) // wide net intended
                {
                    _log.Warning("PartModule " + pm.Value.GetType().FullName + " on " + pm.Key.FlightID +
                                 " threw an exception in OnInitialize: " + e);
                }
            }
        }


        private void RunOnStarts()
        {
            // note to self: it's possible for parts not to be associated with any vessel,
            // such as when constructing a ship in the editor
            var defaultGuid = Guid.NewGuid();

            var groupedByVessel = _targets
                .GroupBy(target => target.Key.Vessel.Return(v => v.ID, defaultGuid), target => target)
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
