extern alias KSP;
using System;
using System.Linq;
using ReeperCommon.Containers;
using ReeperCommon.Logging;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class ScenarioModuleFactory : IScenarioModuleFactory
    {
        private readonly IScenarioModuleConfigNodeRepository _configRepository;
        private readonly IGetTypeIdentifier _typeIdentifierQuery;
        private readonly ILog _log;


        public ScenarioModuleFactory(
            IScenarioModuleConfigNodeRepository configRepository,
            IGetTypeIdentifier typeIdentifierQuery,
            [Name(LogKey.ScenarioModuleFactory)] ILog log)
        {
            if (configRepository == null) throw new ArgumentNullException("configRepository");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (log == null) throw new ArgumentNullException("log");

            _configRepository = configRepository;
            _typeIdentifierQuery = typeIdentifierQuery;
            _log = log;
        }


        public void Create(IProtoScenarioModule psm, Type type)
        {
            if (psm == null) throw new ArgumentNullException("psm");
            if (type == null) throw new ArgumentNullException("type");

            if (psm.moduleName != _typeIdentifierQuery.Get(type).Identifier)
                throw new WrongProtoScenarioModuleForTypeException(psm, _typeIdentifierQuery.Get(type));

            if (psm.moduleRef.Any())
                throw new ModuleRefAlreadyExistsException(psm);

            var configToUse = _configRepository.Retrieve(_typeIdentifierQuery.Get(type));

            if (!configToUse.Any())
                throw new NoConfigNodeFoundException(type);

            if (!configToUse.Single().HasValue("name"))
                throw new ConfigNodeDoesNotSpecifyTypeException(configToUse.Single());

            CreateScenarioModule(psm, configToUse.Single());
        }


        private void CreateScenarioModule(IProtoScenarioModule psm, KSP::ConfigNode config)
        {
            if (config == null) throw new ArgumentNullException("config");

            var runner = KSP::ScenarioRunner.fetch;

            if (runner == null)
                throw new ScenarioRunnerNotFound();

            try
            {
                var psmConfig = psm.GetData(); // GetData returns a reference to their private reference ... ;)
                psmConfig.ClearData();
                config.CopyTo(psmConfig);

                var scenarioModule = runner.AddModule(config);

                if (scenarioModule == null)
                    _log.Error("Failed to AddModule " + psm.moduleName + " with config {0}", psmConfig.ToString());
                else
                {
                    // ScenarioModule does NOT load target scenes itself (to its own field) from supplied ConfigNode, so we must be sure to copy it over
                    scenarioModule.targetScenes = psm.TargetScenes.ToList();

                    if (!scenarioModule.targetScenes.Any()) // shouldn't ever be hit, included as a precaution
                        _log.Error(psm.moduleName +
                            " target scenes not set correctly. Expect game state to be hosed on next save");
                }

                psm.moduleRef = scenarioModule.ToMaybe();
            }
            catch (Exception e) // wide net intended
            {
                _log.Error("Exception while adding ScenarioModule " + psm.moduleName + " to ScenarioRunner: " + e);
            }
        }
    }
}
