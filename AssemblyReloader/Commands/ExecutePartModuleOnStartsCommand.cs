using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Properties;
using AssemblyReloader.Queries;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Commands
{
    public class ExecutePartModuleOnStartsCommand : ICommand, IPartModuleOnStartRunner
    {
        private readonly IGetPartModuleStartState _startState;
        private readonly IGetIsPartPrefab _getIsPartPrefab;
        private readonly IKspFactory _kspFactory;
        private readonly ILog _log;
        private readonly List<PartModule> _partModules = new List<PartModule>();
 
        public ExecutePartModuleOnStartsCommand(
            [NotNull] IGetPartModuleStartState startState,
            [NotNull] IGetIsPartPrefab getIsPartPrefab,
            [NotNull] IKspFactory kspFactory,
            [NotNull] ILog log)
        {
            if (startState == null) throw new ArgumentNullException("startState");
            if (getIsPartPrefab == null) throw new ArgumentNullException("getIsPartPrefab");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (log == null) throw new ArgumentNullException("log");
            _startState = startState;
            _getIsPartPrefab = getIsPartPrefab;
            _kspFactory = kspFactory;
            _log = log;
        }


        public void Execute()
        {
            var needOnStart = _partModules
                .Where(pm => !_getIsPartPrefab.Get(_kspFactory.Create(pm.part))) // don't run OnStart for any PartModules on part prefabs
                .ToList();

            _log.Verbose(needOnStart.Count + " part modules");

            needOnStart.ForEach(RunOnStart);
            ClearPartModuleTargets();
        }


        private void RunOnStart([NotNull] PartModule pm)
        {
            if (pm == null) throw new ArgumentNullException("pm");

            var startingState = _startState.Get(pm.vessel != null ? Maybe<IVessel>.With(_kspFactory.Create(pm.vessel)) : Maybe<IVessel>.None);

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

        public void ClearPartModuleTargets()
        {
            _partModules.Clear();
        }
    }
}
