using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.command.impl;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Containers;
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
        private readonly IGetTypeIdentifier _typeIdentifierQuery;
        private readonly ILog _log;


        public CommandCreateScenarioModuleConfigNode(
            MonoBehaviour mbToBeDestroyed,
            IScenarioModuleConfigNodeRepository configRepository,
            IGetProtoScenarioModules protoScenarioModuleQuery,
            IScenarioModuleSettings scenarioModuleSettings,
            IGetTypeIdentifier typeIdentifierQuery,
            [Name(LogKey.ScenarioModuleConfigNodeUpdater)] ILog log)
        {
            if (mbToBeDestroyed == null) throw new ArgumentNullException("mbToBeDestroyed");
            if (configRepository == null) throw new ArgumentNullException("configRepository");
            if (protoScenarioModuleQuery == null) throw new ArgumentNullException("protoScenarioModuleQuery");
            if (scenarioModuleSettings == null) throw new ArgumentNullException("scenarioModuleSettings");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (log == null) throw new ArgumentNullException("log");

            _mbToBeDestroyed = mbToBeDestroyed;
            _configRepository = configRepository;
            _protoScenarioModuleQuery = protoScenarioModuleQuery;
            _scenarioModuleSettings = scenarioModuleSettings;
            _typeIdentifierQuery = typeIdentifierQuery;
            _log = log;
        }


        public override void Execute()
        {
            var scenarioModule = _mbToBeDestroyed as ScenarioModule;

            if (scenarioModule == null)
                return;

            var psm = GetProtoScenarioModuleFor(scenarioModule);

            if (!psm.Any())
                throw new ArgumentException("ScenarioModule " + scenarioModule.GetType().FullName +
                " does not have a corresponding ProtoScenarioModule reference");

            if (_scenarioModuleSettings.SaveScenarioModuleBeforeDestroying)
                CreateConfigNodeFromCurrentState(psm.Single(), scenarioModule);
            else
                StoreLastConfigNodeGameUsed(psm.Single(), scenarioModule);
        }


        private void CreateConfigNodeFromCurrentState(IProtoScenarioModule psm, ScenarioModule scenarioModule)
        {
            try
            {
                var config = new ConfigNode(psm.GetData().name);

                scenarioModule.Save(config);
                _configRepository.Store(_typeIdentifierQuery.Get(scenarioModule.GetType()), config);
                _log.Verbose("Stored updated ConfigNode for " + psm.moduleName);
            }
            catch (Exception e)
            {
                _log.Error("Failed to create ConfigNode from ScenarioModule: " + psm.moduleName + "; exception: " + e);
                _log.Error("Will use last known ConfigNode from Game instead");

                StoreLastConfigNodeGameUsed(psm, scenarioModule);
            }
        }


        private void StoreLastConfigNodeGameUsed(IProtoScenarioModule psm, ScenarioModule scenarioModule)
        {
            _configRepository.Store(_typeIdentifierQuery.Get(scenarioModule.GetType()), psm.GetData().CreateCopy());
            _log.Verbose("Stored last game ConfigNode for " + psm.moduleName);
        }


        private Maybe<IProtoScenarioModule> GetProtoScenarioModuleFor(ScenarioModule scenarioModule)
        {
            var psmWithThisRef =
                _protoScenarioModuleQuery.Get(scenarioModule.GetType())
                .FirstOrDefault(
                    psm => psm.moduleRef.Any() && ReferenceEquals(psm.moduleRef.Single(), scenarioModule));

            return psmWithThisRef != null
                ? Maybe<IProtoScenarioModule>.With(psmWithThisRef) : Maybe<IProtoScenarioModule>.None;
        }
    }
}
