using System;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Destruction;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Providers.Game;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders
{
    public class ScenarioModuleUnloader : IScenarioModuleUnloader
    {
        private readonly ICurrentGameProvider _gameProvider;
        private readonly IScenarioRunnerComponentQuery _scenarioRunnerComponentQuery;
        private readonly IProtoScenarioModuleProvider _psmProvider;
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly bool _reuseConfigNode;
        private readonly ILog _log;

        public ScenarioModuleUnloader([NotNull] ICurrentGameProvider gameProvider,
            [NotNull] IScenarioRunnerComponentQuery scenarioRunnerComponentQuery,
            [NotNull] IProtoScenarioModuleProvider psmProvider,
            [NotNull] IUnityObjectDestroyer objectDestroyer,
            bool reuseConfigNode,
            [NotNull] ILog log)
        {
            if (gameProvider == null) throw new ArgumentNullException("gameProvider");
            if (scenarioRunnerComponentQuery == null) throw new ArgumentNullException("scenarioRunnerComponentQuery");
            if (psmProvider == null) throw new ArgumentNullException("psmProvider");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _gameProvider = gameProvider;
            _scenarioRunnerComponentQuery = scenarioRunnerComponentQuery;
            _psmProvider = psmProvider;
            _objectDestroyer = objectDestroyer;
            _reuseConfigNode = reuseConfigNode;
            _log = log;
        }


        public void Unload([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            var psms = _psmProvider.Get(type).ToArray();

            if (psms.Length > 1)
                throw new Exception("Found multiple ProtoScenarioModules for " + type.FullName +
                                    "; this should be impossible");

            if (psms.Any())
                UninstallScenarioModule(type, psms.Single());
        }


        private void UninstallScenarioModule(Type type, [NotNull] IProtoScenarioModule psm)
        {
            if (psm == null) throw new ArgumentNullException("psm");

            if (!psm.moduleRef.Any())
            {
                _log.Warning("Psm.ModuleRef is not set to anything for " + psm.moduleName);
                return; //? could be that the player has uninstalled a mod (or the dev has renamed their ScenarioModule)
                        // leaving ProtoScenarioModules which are never successfully initialized with moduleRefs
            }


            var game = _gameProvider.Get().FirstOrDefault();

            if (game == null)
                throw new InvalidOperationException("Current game is null");


            // take a snapshot of the current state of the ScenarioModule so we can reuse it to load
            // the next version
            var snapshot = new ConfigNode("SCENARIO");
            var sm = GetScenarioModuleInstanceFromRunner(type);

            bool snapshotSuccess = false;

            if (_reuseConfigNode)
                snapshotSuccess = TryToSaveScenarioModuleState(sm.First(), snapshot);

            if (!game.RemoveProtoScenarioModule(type))
                throw new Exception("Failed to remove proto scenario module of " + type.FullName);

            if (_reuseConfigNode && snapshotSuccess)
                game.AddProtoScenarioModule(snapshot);
            else game.AddProtoScenarioModule(type, psm.TargetScenes);

            _log.Normal("Destroying ScenarioModule " + type.FullName + " (called " + psm.moduleName + ")");

            _objectDestroyer.Destroy(sm.First());
            psm.moduleRef = Maybe<ScenarioModule>.None;
        }


        private Maybe<ScenarioModule> GetScenarioModuleInstanceFromRunner(Type type)
        {

            var smInstancesOfType = _scenarioRunnerComponentQuery.Get(type).Where(c => c is ScenarioModule).ToList();

            // game enforces one unique instance so somebody has done something naughty and we can't be sure how to proceed
            if (smInstancesOfType.Count > 1)
                throw new InvalidOperationException("Found multiple ScenarioModules of type " + type.FullName + " on ScenarioRunner");


            return smInstancesOfType.Any() ? Maybe<ScenarioModule>.With(smInstancesOfType.Single() as ScenarioModule) : Maybe<ScenarioModule>.None;
        }


        private bool TryToSaveScenarioModuleState(ScenarioModule sm, ConfigNode node)
        {
            try
            {
                sm.Save(node);

                return true;
            }
            catch (Exception)
            {
                _log.Warning("Failed to save snapshot of " + sm.GetType().FullName +
                             "; default ConfigNode will be used for next instance");

                return false;
            }
        }
    }
}
