using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector;
using AssemblyReloader.Unsorted;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IScenarioModuleFactory))]
    public class ScenarioModuleFactory : IScenarioModuleFactory
    {
        private readonly IScenarioModuleConfigNodeRepository _configRepository;
        private readonly IGetTypeIdentifier _typeIdentifierQuery;
        private readonly IScenarioRunnerProvider _scenarioRunnerProvider;
        private readonly ILog _log;


        public ScenarioModuleFactory(
            IScenarioModuleConfigNodeRepository configRepository,
            IGetTypeIdentifier typeIdentifierQuery,
            IScenarioRunnerProvider scenarioRunnerProvider,
            [Name(LogKeys.ScenarioModuleFactory)] ILog log)
        {
            if (configRepository == null) throw new ArgumentNullException("configRepository");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (scenarioRunnerProvider == null) throw new ArgumentNullException("scenarioRunnerProvider");
            if (log == null) throw new ArgumentNullException("log");

            _configRepository = configRepository;
            _typeIdentifierQuery = typeIdentifierQuery;
            _scenarioRunnerProvider = scenarioRunnerProvider;
            _log = log;
        }


        public Maybe<ScenarioModule> Create(IProtoScenarioModule psm, Type type)
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

            return CreateScenarioModule(psm, type, configToUse.Single());
        }


        private Maybe<ScenarioModule> CreateScenarioModule(IProtoScenarioModule psm, Type type, ConfigNode config)
        {
            var runner = _scenarioRunnerProvider.Get();

            if (!runner.Any())
                throw new ScenarioRunnerNotFound();

            // this is a bit ugly, but the loaded list of ScenarioModules in ScenarioRunner is private and therefore
            // inaccessible. We could easily create it ourselves, except that if any client plugin were to use
            // the static ScenarioRunner.GetLoadedModules() which would return incorrect results
            //
            // todo: consider intercepting calls to GetLoadedModules() to avoid chicken chance

            var currentPsms = ScenarioRunner.GetUpdatedProtoModules();

            //var scenarioModule = runner.Single().gameObject.a

            return Maybe<ScenarioModule>.None;
        }
    }
}
