extern alias Cecil96;
using System;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin
{

// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadReloadablePlugin : Command
    {
        private readonly ReeperAssembly _reeperAssembly;
        private readonly IReeperAssemblyLoader _loader;
        private readonly SignalPluginCannotBeLoaded _failSignal;
        private readonly SignalPluginWasLoaded _pluginLoadedSignal;
        private readonly ILog _log;

        public CommandLoadReloadablePlugin(
            ReeperAssembly reeperAssembly,
            IReeperAssemblyLoader loader,
            SignalPluginCannotBeLoaded failSignal,
            SignalPluginWasLoaded pluginLoadedSignal,
            ILog log)
        {
            if (reeperAssembly == null) throw new ArgumentNullException("reeperAssembly");
            if (loader == null) throw new ArgumentNullException("loader");
            if (failSignal == null) throw new ArgumentNullException("failSignal");
            if (pluginLoadedSignal == null) throw new ArgumentNullException("pluginLoadedSignal");
            if (log == null) throw new ArgumentNullException("log");

            _reeperAssembly = reeperAssembly;
            _loader = loader;
            _failSignal = failSignal;
            _pluginLoadedSignal = pluginLoadedSignal;
            _log = log;
        }

        public override void Execute()
        {
            _log.Verbose("Loading plugin " + _reeperAssembly.File.name);

            try
            {
                var handle = _loader.Load(_reeperAssembly);
                _pluginLoadedSignal.Dispatch(handle);
            }
            catch (Exception e)
            {
                _log.Error("Exception while loading plugin: " + e);

                _failSignal.Dispatch("Exception while loading plugin! See the log for details. " + e.TargetSite.Name);

                _log.Error("Plugin was NOT loaded successfully!");
                Fail();
                return;
            }

            _log.Verbose("Plugin loaded successfully!");
        }
    }
}
