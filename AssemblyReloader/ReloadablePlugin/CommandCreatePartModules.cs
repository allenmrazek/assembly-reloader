using System;
using System.Collections.Generic;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCreatePartModules : Command
    {
        private readonly IPartModuleLoader _partModuleLoader;
        private readonly ILoadedAssemblyHandle _handle;
        private readonly SignalLoadersFinished _loadersFinishedSignal;
        private readonly SignalPartModuleCreated _createdSignal;
        private readonly ILog _log;

        private readonly List<PartModule> _targets = new List<PartModule>();


        public CommandCreatePartModules(
            IPartModuleLoader partModuleLoader, 
            ILoadedAssemblyHandle handle,
            SignalLoadersFinished loadersFinishedSignal,
            SignalPartModuleCreated createdSignal,
            ILog log)
        {
            if (partModuleLoader == null) throw new ArgumentNullException("partModuleLoader");
            if (handle == null) throw new ArgumentNullException("handle");
            if (loadersFinishedSignal == null) throw new ArgumentNullException("loadersFinishedSignal");
            if (createdSignal == null) throw new ArgumentNullException("createdSignal");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleLoader = partModuleLoader;
            _handle = handle;
            _loadersFinishedSignal = loadersFinishedSignal;
            _createdSignal = createdSignal;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Creating PartModules");

            _createdSignal.AddListener(OnPartModuleCreated);
            _partModuleLoader.CreatePartModules(_handle);
            _createdSignal.RemoveListener(OnPartModuleCreated);

            _loadersFinishedSignal.AddListener(OnLoadersFinished);
            Retain();
        }


        private void OnPartModuleCreated(PartModule pm, PartModuleDescriptor descriptor)
        {
            if (pm == null) throw new ArgumentNullException("pm");
            if (descriptor == null) throw new ArgumentNullException("descriptor");

            // todo: filter out prefab PartModule

            _targets.Add(pm);
        }


        private void OnLoadersFinished()
        {
            _loadersFinishedSignal.RemoveListener(OnLoadersFinished);
            
            Release();

            _log.Warning("CommandRunPartModuleOnStarts ready to run onstarts on " + _targets.Count + " targets");
        }
    }
}
