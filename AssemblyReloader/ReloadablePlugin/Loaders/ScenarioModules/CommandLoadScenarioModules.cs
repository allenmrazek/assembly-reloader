using System;
using AssemblyReloader.Game;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{

// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandLoadScenarioModules : Command
    {
        private readonly ILoadedAssemblyHandle _loadedHandle;
        private readonly IScenarioModuleLoader _scenarioModuleLoader;
        private readonly IScenarioModuleSettings _scenarioModuleSettings;
        private readonly ILog _log;


        public CommandLoadScenarioModules(
            ILoadedAssemblyHandle loadedHandle,
            IScenarioModuleLoader scenarioModuleLoader,
            IScenarioModuleSettings scenarioModuleSettings,
            ILog log)
        {
            if (loadedHandle == null) throw new ArgumentNullException("loadedHandle");
            if (scenarioModuleLoader == null) throw new ArgumentNullException("scenarioModuleLoader");
            if (scenarioModuleSettings == null) throw new ArgumentNullException("scenarioModuleSettings");
            if (log == null) throw new ArgumentNullException("log");

            _loadedHandle = loadedHandle;
            _scenarioModuleLoader = scenarioModuleLoader;
            _scenarioModuleSettings = scenarioModuleSettings;
            _log = log;
        }


        public override void Execute()
        {
            if (!_scenarioModuleSettings.CreateScenarioModulesImmediately)
                return;

            _log.Debug("Creating ScenarioModules");

            _scenarioModuleLoader.Load(_loadedHandle);

            _log.Debug("Finished creating ScenarioModules");
        }
    }
}
