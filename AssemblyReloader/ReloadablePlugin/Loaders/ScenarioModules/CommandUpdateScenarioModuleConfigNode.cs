using System;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using ReeperCommon.Logging;
using UnityEngine;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class CommandCreateScenarioModuleConfigNode : Command
    {
        private readonly MonoBehaviour _mbToBeDestroyed;
        private readonly IScenarioModuleConfigNodeRepository _configRepository;
        private readonly IGetProtoScenarioModules _protoScenarioModuleQuery;
        private readonly IScenarioModuleSettings _scenarioModuleSettings;
        private readonly ILog _log;


        public CommandCreateScenarioModuleConfigNode(
            MonoBehaviour mbToBeDestroyed,
            IScenarioModuleConfigNodeRepository configRepository,
            IGetProtoScenarioModules protoScenarioModuleQuery,
            IScenarioModuleSettings scenarioModuleSettings,
            ILog log)
        {
            if (mbToBeDestroyed == null) throw new ArgumentNullException("mbToBeDestroyed");
            if (configRepository == null) throw new ArgumentNullException("configRepository");
            if (protoScenarioModuleQuery == null) throw new ArgumentNullException("protoScenarioModuleQuery");
            if (scenarioModuleSettings == null) throw new ArgumentNullException("scenarioModuleSettings");
            if (log == null) throw new ArgumentNullException("log");

            _mbToBeDestroyed = mbToBeDestroyed;
            _configRepository = configRepository;
            _protoScenarioModuleQuery = protoScenarioModuleQuery;
            _scenarioModuleSettings = scenarioModuleSettings;
            _log = log;
        }


        public override void Execute()
        {
            var scenarioModule = _mbToBeDestroyed as ScenarioModule;

            if (scenarioModule == null)
                return;

            if (_scenarioModuleSettings.SaveScenarioModuleBeforeDestroying)
                UpdateConfigNode();
        }


        private void UpdateConfigNode()
        {
            _log.Normal("Here I would update ScenarioModule ConfigNode");
        }
    }
}
