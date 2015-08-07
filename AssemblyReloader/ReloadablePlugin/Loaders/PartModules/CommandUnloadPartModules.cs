using System;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once UnusedMember.Global
    public class CommandUnloadPartModules : Command
    {
        private readonly ILoadedAssemblyHandle _handle;
        private readonly IPartModuleUnloader _partModuleUnloader;
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly ILog _log;

        public CommandUnloadPartModules(
            ILoadedAssemblyHandle handle,
            IPartModuleUnloader partModuleUnloader,
            IPartModuleSettings partModuleSettings,
            ILog log)
        {
            if (handle == null) throw new ArgumentNullException("handle");
            if (partModuleUnloader == null) throw new ArgumentNullException("partModuleUnloader");
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (log == null) throw new ArgumentNullException("log");

            _handle = handle;
            _partModuleUnloader = partModuleUnloader;
            _partModuleSettings = partModuleSettings;
            _log = log;
        }


        public override void Execute()
        {
            _log.Debug("Unloading PartModules...");

            try
            {
                _partModuleUnloader.Unload(_handle, !_partModuleSettings.ReloadPartModuleInstancesImmediately);
                _log.Debug("Finished unloading PartModules");
            }
            catch (Exception e)
            {
                _log.Error("Exception occurred while unloading PartModules: " + e);
                // todo: popup?
            }
        }
    }
}
