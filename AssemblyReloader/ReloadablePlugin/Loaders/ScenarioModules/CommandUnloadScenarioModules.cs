using System;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandUnloadScenarioModules : Command
    {
        private readonly IScenarioModuleUnloader _scenarioModuleUnloader;
        private readonly ILog _log;


        public CommandUnloadScenarioModules(
            IScenarioModuleUnloader scenarioModuleUnloader,
            ILog log)
        {
            if (scenarioModuleUnloader == null) throw new ArgumentNullException("scenarioModuleUnloader");
            if (log == null) throw new ArgumentNullException("log");

            _scenarioModuleUnloader = scenarioModuleUnloader;
            _log = log;
        }


        public override void Execute()
        {
            _log.Debug("Destroying ScenarioModules");

        }
    }
}
