using System;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules;
using AssemblyReloader.ReloadablePlugin.Loaders.VesselModules;
using JetBrains.Annotations;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    class CommandClearConfigNodeSnapshots : Command
    {
        private readonly IPartModuleConfigNodeSnapshotRepository _pmConfigs;
        private readonly IScenarioModuleConfigNodeRepository _smConfigs;
        private readonly IVesselModuleConfigNodeRepository _vmConfigs;
        private readonly ILog _log;

        public CommandClearConfigNodeSnapshots(
            [NotNull] IPartModuleConfigNodeSnapshotRepository pmConfigs,
            [NotNull] IScenarioModuleConfigNodeRepository smConfigs, 
            [NotNull] IVesselModuleConfigNodeRepository vmConfigs, 
            [NotNull] ILog log)
        {
            if (pmConfigs == null) throw new ArgumentNullException("pmConfigs");
            if (smConfigs == null) throw new ArgumentNullException("smConfigs");
            if (vmConfigs == null) throw new ArgumentNullException("vmConfigs");
            if (log == null) throw new ArgumentNullException("log");
            _pmConfigs = pmConfigs;
            _smConfigs = smConfigs;
            _vmConfigs = vmConfigs;
            _log = log;
        }


        public override void Execute()
        {
            _log.Debug("Clearing ConfigNode snapshots");

            ClearPartModuleSnapshots();
            ClearScenarioModuleSnapshots();
            ClearVesselModuleSnapshots();
        }



        private void ClearPartModuleSnapshots()
        {
            var count = _pmConfigs.Count();

            if (count == 0) return;

            _log.Verbose("Clearing " + count + " unused PartModule ConfigNode snapshots");
            _pmConfigs.Clear();
        }


        private void ClearScenarioModuleSnapshots()
        {
            var count = _smConfigs.Count();

            if (count == 0) return;

            _log.Verbose("Clearing " + count + " unused ScenarioModule ConfigNode snapshots");
            _smConfigs.Clear();
        }

        private void ClearVesselModuleSnapshots()
        {
            var count = _vmConfigs.Count();

            if (count == 0) return;

            _log.Verbose("Clearing " + count + " unused VesselModule ConfigNode snapshots");
            _vmConfigs.Clear();
        }
    }
}
