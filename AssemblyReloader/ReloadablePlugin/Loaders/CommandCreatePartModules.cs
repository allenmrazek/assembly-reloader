using System;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.PartModules;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public class CommandCreatePartModules : Command
    {
        private readonly IPartModuleLoader _partModuleLoader;
        private readonly ILoadedAssemblyHandle _handle;
        private readonly ILog _log;

        public CommandCreatePartModules(IPartModuleLoader partModuleLoader, ILoadedAssemblyHandle handle, ILog log)
        {
            if (partModuleLoader == null) throw new ArgumentNullException("partModuleLoader");
            if (handle == null) throw new ArgumentNullException("handle");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleLoader = partModuleLoader;
            _handle = handle;
            _log = log;
        }


        public override void Execute()
        {
            // create PartModules for prefabs
            _log.Verbose("Loading prefab modules");
            _partModuleLoader.LoadPrefabs(_handle);

            // hotswap PartModules here, if enabled
            _log.Verbose("Creating live modules");
            _partModuleLoader.LoadInstances(_handle);
        }
    }
}
