using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Loaders.PartModuleLoader;
using AssemblyReloader.Queries;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Commands
{
    public class ExecutePartModuleOnStartsCommand : ICommand, IPartModuleOnStartRunner
    {
        private readonly IPartModuleStartStateProvider _startStateProvider;
        private readonly IPartIsPrefabQuery _partIsPrefabQuery;
        private readonly IKspFactory _kspFactory;
        private readonly ILog _log;
        private readonly List<PartModule> _partModules = new List<PartModule>();
 
        public ExecutePartModuleOnStartsCommand(
            [NotNull] IPartModuleStartStateProvider startStateProvider,
            [NotNull] IPartIsPrefabQuery partIsPrefabQuery,
            [NotNull] IKspFactory kspFactory,
            [NotNull] ILog log)
        {
            if (startStateProvider == null) throw new ArgumentNullException("startStateProvider");
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (log == null) throw new ArgumentNullException("log");
            _startStateProvider = startStateProvider;
            _partIsPrefabQuery = partIsPrefabQuery;
            _kspFactory = kspFactory;
            _log = log;
        }


        public void Execute()
        {
            var needOnStart = _partModules
                .Where(pm => !_partIsPrefabQuery.Get(_kspFactory.Create(pm.part))) // don't run OnStart for any PartModules on part prefabs
                .ToList();

            _log.Verbose(needOnStart.Count + " part modules");

            needOnStart.ForEach(RunOnStart);
            Clear();
        }


        private void RunOnStart([NotNull] PartModule pm)
        {
            if (pm == null) throw new ArgumentNullException("pm");

            var startingState = _startStateProvider.Get(pm.vessel != null ? Maybe<IVessel>.With(_kspFactory.Create(pm.vessel)) : Maybe<IVessel>.None);

            try
            {
                _log.Debug("Running OnStart for " + pm.name + " on " + pm.part.partInfo.name);
                pm.OnStart(startingState);
            }
            catch (Exception e)
            {
                _log.Error("Exception while running OnStart on " + pm.name + ": " + e);
            }
        }


        public void Add([NotNull] PartModule target)
        {
            if (target == null) throw new ArgumentNullException("target");

            if (_partModules.Contains(target))
                throw new Exception("target already in list");

            _partModules.Add(target);
        }

        public void Clear()
        {
            _partModules.Clear();
        }
    }
}
