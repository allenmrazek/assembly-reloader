using System;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadVesselModules : Command
    {
        private readonly ILoadedAssemblyHandle _loadedAssembly;
        private readonly IVesselModuleLoader _vesselModuleLoader;
        private readonly ILog _log;


        public CommandLoadVesselModules(
            ILoadedAssemblyHandle loadedAssembly, 
            IVesselModuleLoader vesselModuleLoader,
            ILog log)
        {
            if (loadedAssembly == null) throw new ArgumentNullException("loadedAssembly");
            if (vesselModuleLoader == null) throw new ArgumentNullException("vesselModuleLoader");
            if (log == null) throw new ArgumentNullException("log");

            _loadedAssembly = loadedAssembly;
            _vesselModuleLoader = vesselModuleLoader;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Loading VesselModule wrappers");
            _vesselModuleLoader.Load(_loadedAssembly);
            _log.Verbose("Done loading VesselModule wrappers");
        }
    }
}
