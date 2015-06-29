using System;
using System.Linq;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Game.Queries;
using AssemblyReloader.Properties;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    public class ScenarioModuleUnloader : IScenarioModuleUnloader
    {
        private readonly IGameObjectComponentQuery _gameObjectComponentQuery;
        private readonly IGetProtoScenarioModules _psmProvider;
        private readonly IUnityObjectDestroyer _objectDestroyer;
        private readonly Func<bool> _saveBeforeDestroying;
        private readonly IScenarioModuleSnapshotGenerator _snapshotGenerator;
        private readonly ILog _log;

        public ScenarioModuleUnloader(
            [NotNull] IGameObjectComponentQuery gameObjectComponentQuery,
            [NotNull] IGetProtoScenarioModules psmProvider,
            [NotNull] IUnityObjectDestroyer objectDestroyer, 
            [NotNull] Func<bool> saveBeforeDestroying,
            [NotNull] IScenarioModuleSnapshotGenerator snapshotGenerator,
            [NotNull] ILog log)
        {
            if (gameObjectComponentQuery == null) throw new ArgumentNullException("gameObjectComponentQuery");
            if (psmProvider == null) throw new ArgumentNullException("psmProvider");
            if (objectDestroyer == null) throw new ArgumentNullException("objectDestroyer");
            if (saveBeforeDestroying == null) throw new ArgumentNullException("saveBeforeDestroying");
            if (snapshotGenerator == null) throw new ArgumentNullException("snapshotGenerator");
            if (log == null) throw new ArgumentNullException("log");

            _gameObjectComponentQuery = gameObjectComponentQuery;
            _psmProvider = psmProvider;
            _objectDestroyer = objectDestroyer;
            _saveBeforeDestroying = saveBeforeDestroying;
            _snapshotGenerator = snapshotGenerator;
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


            var sm = GetScenarioModuleInstanceFromRunner(type);

            if (!sm.Any())
                throw new Exception("Did not find ScenarioModule of type " + type.FullName + "(" + psm.moduleName + ") on ScenarioRunner");


            if (_saveBeforeDestroying())
                _snapshotGenerator.Snapshot(sm.Single(), psm);

            _log.Normal("Destroying ScenarioModule " + type.FullName + " (called " + psm.moduleName + ")");

            _objectDestroyer.Destroy(sm.First());
            psm.moduleRef = Maybe<ScenarioModule>.None;
        }


        private Maybe<ScenarioModule> GetScenarioModuleInstanceFromRunner(Type type)
        {

            var smInstancesOfType = _gameObjectComponentQuery.Get(type).Where(c => c is ScenarioModule).ToList();

            // game enforces one unique instance so somebody has done something naughty and we can't be sure how to proceed
            if (smInstancesOfType.Count > 1)
                throw new InvalidOperationException("Found multiple ScenarioModules of type " + type.FullName + " on ScenarioRunner");

            return smInstancesOfType.Any() ? Maybe<ScenarioModule>.With(smInstancesOfType.Single() as ScenarioModule) : Maybe<ScenarioModule>.None;
        }
    }
}
