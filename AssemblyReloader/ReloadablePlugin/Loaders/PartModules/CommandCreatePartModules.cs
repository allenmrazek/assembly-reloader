using System;
using System.Collections.Generic;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
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
        private readonly ILog _log;

        private readonly List<PartModule> _targets = new List<PartModule>();


        public CommandCreatePartModules(
            IPartModuleLoader partModuleLoader, 
            ILoadedAssemblyHandle handle,
            IPartModuleSettings partModuleSettings,
            IPartModuleConfigNodeSnapshotRepository snapshotRepository,
            SignalLoadersFinished loadersFinishedSignal,
            SignalPartModuleCreated createdSignal,
            ILog log)
        {
            if (partModuleLoader == null) throw new ArgumentNullException("partModuleLoader");
            if (handle == null) throw new ArgumentNullException("handle");
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (snapshotRepository == null) throw new ArgumentNullException("snapshotRepository");
            if (loadersFinishedSignal == null) throw new ArgumentNullException("loadersFinishedSignal");
            if (createdSignal == null) throw new ArgumentNullException("createdSignal");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleLoader = partModuleLoader;
            _handle = handle;
            _partModuleSettings = partModuleSettings;
            _snapshotRepository = snapshotRepository;
            _loadersFinishedSignal = loadersFinishedSignal;
            _createdSignal = createdSignal;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Creating PartModules");

            _createdSignal.AddListener(OnPartModuleCreated);
            _partModuleLoader.Load(_handle, !_partModuleSettings.ReplacePartModulesInstancesImmediately);
            _createdSignal.RemoveListener(OnPartModuleCreated);

            _loadersFinishedSignal.AddListener(OnLoadersFinished);
            Retain();
        }


        private void OnPartModuleCreated(IPart part, PartModule pm, PartModuleDescriptor descriptor)
        {
            if (part == null) throw new ArgumentNullException("part");
            if (pm == null) throw new ArgumentNullException("pm");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            // todo: filter out prefab PartModule

            _targets.Add(pm);
        }


        private void OnLoadersFinished()
        {
            _loadersFinishedSignal.RemoveListener(OnLoadersFinished);
            _snapshotRepository.Clear();

            Release();

            _log.Warning("CommandRunPartModuleOnStarts ready to run onstarts on " + _targets.Count + " targets");
        }
    }
}
