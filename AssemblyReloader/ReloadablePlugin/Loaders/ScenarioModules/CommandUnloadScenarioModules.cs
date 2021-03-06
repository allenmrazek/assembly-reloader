using System;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.command.impl;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandUnloadScenarioModules : Command
    {
        private readonly ILoadedAssemblyHandle _loadedHandle;
        private readonly IQueryScenarioModulesCanRunInCurrentScene _scenarioModulesExistInSceneQuery;
        private readonly IScenarioModuleUnloader _scenarioModuleUnloader;
        private readonly ILog _log;


        public CommandUnloadScenarioModules(
            ILoadedAssemblyHandle loadedHandle,
            IQueryScenarioModulesCanRunInCurrentScene scenarioModulesExistInSceneQuery,
            IScenarioModuleUnloader scenarioModuleUnloader,
            ILog log)
        {
            if (loadedHandle == null) throw new ArgumentNullException("loadedHandle");
            if (scenarioModulesExistInSceneQuery == null)
                throw new ArgumentNullException("scenarioModulesExistInSceneQuery");
            if (scenarioModuleUnloader == null) throw new ArgumentNullException("scenarioModuleUnloader");
            if (log == null) throw new ArgumentNullException("log");

            _loadedHandle = loadedHandle;
            _scenarioModulesExistInSceneQuery = scenarioModulesExistInSceneQuery;
            _scenarioModuleUnloader = scenarioModuleUnloader;
            _log = log;
        }


        public override void Execute()
        {
            if (!_scenarioModulesExistInSceneQuery.Get())
                return;

            _log.Debug("Destroying ScenarioModules");

            _scenarioModuleUnloader.Unload(_loadedHandle);

            _log.Debug("Finishing destroyed ScenarioModules");
        }
    }
}
