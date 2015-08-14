using System;
using AssemblyReloader.Game;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public class CommandUnloadVesselModules : Command
    {
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IVesselModuleUnloader _vesselModuleUnloader;
        private readonly ILog _log;


        public CommandUnloadVesselModules(
            ILoadedAssemblyHandle handle, 
            IVesselModuleUnloader vesselModuleUnloader,
            ILog log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (vesselModuleUnloader == null) throw new ArgumentNullException("vesselModuleUnloader");
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _vesselModuleUnloader = vesselModuleUnloader;
            _log = log;
        }


        public override void Execute()
        {
            _log.Verbose("Unloading VesselModules");
            _vesselModuleUnloader.Unload(_handle);
        }



    }
}
