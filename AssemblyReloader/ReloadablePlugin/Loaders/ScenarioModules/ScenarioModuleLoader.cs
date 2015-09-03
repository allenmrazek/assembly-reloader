extern alias KSP;
using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.ReloadablePlugin.Loaders.Addons;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.implicitBind;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Loaders.ScenarioModules
{
    [Implements(typeof(IScenarioModuleLoader))]
    public class ScenarioModuleLoader : IScenarioModuleLoader
    {
        private static readonly KSP::GameScenes[] ValidScenarioModuleScenes =
        {
            KSP::GameScenes.EDITOR, 
            KSP::GameScenes.FLIGHT,
            KSP::GameScenes.SPACECENTER, 
            KSP::GameScenes.TRACKSTATION
        };

        private readonly IGetTypesDerivedFrom<KSP::ScenarioModule> _smTypeQuery;
        private readonly IGetProtoScenarioModules _protoScenarioModuleQuery;
        private readonly IGetCurrentGameScene _gameSceneQuery;
        private readonly IGetAttributesOfType<KSP::KSPScenario> _scenarioAttributeQuery;
        private readonly IScenarioModuleFactory _smFactory;
        private readonly IGetTypeIdentifier _typeIdentifierQuery;
        private readonly ILog _log;


        public ScenarioModuleLoader(
            IGetTypesDerivedFrom<KSP::ScenarioModule> smTypeQuery,
            IGetProtoScenarioModules protoScenarioModuleQuery,
            IGetCurrentGameScene gameSceneQuery,
            IGetAttributesOfType<KSP::KSPScenario> scenarioAttributeQuery,
            IScenarioModuleFactory smFactory,
            IGetTypeIdentifier typeIdentifierQuery,
            [Name(LogKey.ScenarioModuleLoader)] ILog log)
        {
            if (smTypeQuery == null) throw new ArgumentNullException("smTypeQuery");
            if (protoScenarioModuleQuery == null) throw new ArgumentNullException("protoScenarioModuleQuery");
            if (gameSceneQuery == null) throw new ArgumentNullException("gameSceneQuery");
            if (scenarioAttributeQuery == null) throw new ArgumentNullException("scenarioAttributeQuery");
            if (smFactory == null) throw new ArgumentNullException("smFactory");
            if (typeIdentifierQuery == null) throw new ArgumentNullException("typeIdentifierQuery");
            if (log == null) throw new ArgumentNullException("log");

            _smTypeQuery = smTypeQuery;
            _protoScenarioModuleQuery = protoScenarioModuleQuery;
            _gameSceneQuery = gameSceneQuery;
            _scenarioAttributeQuery = scenarioAttributeQuery;
            _smFactory = smFactory;
            _typeIdentifierQuery = typeIdentifierQuery;
            _log = log;
        }


        public void Load(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            if (!ValidScenarioModuleScenes.Contains(_gameSceneQuery.Get()))
                return;

            if (NameCollisionsExist(handle))
            {
                _log.Error("Cannot load ScenarioModules due to name collisions");
                return;
            }

            foreach (var smType in _smTypeQuery.Get(handle.LoadedAssembly.assembly))
                LoadInstanceForEachProtoScenarioModule(smType);
        }


        /// <summary>
        /// It's possible for ScenarioModules in different namespaces with the same name to 
        /// confuse us. They'll confuse the game too so things are going to be broken anyway and
        /// poking at the pieces is a bad plan
        /// 
        /// TODO: come up with a better more elegant fix
        /// TODO: check other loaded assemblies also for collisions?
        /// </summary>
        /// <param name="handle"></param>
        /// <returns></returns>
        private bool NameCollisionsExist(ILoadedAssemblyHandle handle)
        {
            var collisions = _smTypeQuery.Get(handle.LoadedAssembly.assembly)
                .GroupBy(smType => _typeIdentifierQuery.Get(smType).Identifier, smType => smType)
                .Where(grouping => grouping.Count() > 1)
                .ToList();

            if (collisions.Any())
                _log.Error("ScenarioModule name collisions! Collisions: " +
                           string.Join("\n",
                               collisions
                               .Select(g => string.Join(", ", 
                                   g.Select(t => t.FullName)
                                   .ToArray()))
                               .ToArray()));

            return collisions.Count != 0;
        }


        private void LoadInstanceForEachProtoScenarioModule(Type smType)
        {
            // there should be realistically only one but nothing technically prevents ScenarioModules
            // from different namespaces but the same type name from confusing us ... hmm...
            //
            // todo: recreate ProtoScenarioModules so we don't get confused?

            foreach (var psm in _protoScenarioModuleQuery.Get(smType))
                Load(smType, psm);
        }


        private void Load(Type smType, IProtoScenarioModule psm)
        {
            // come up with target scenes by looking at KSPScenario on the type. If that doesn't exist,
            // look instead into a PSM with the right identifier (this is the legacy method)
            //
            // we need to do this because the user might've decided to change them while writing
            // the ScenarioModule and the old ProtoScenarioModule would then be out of date
            var kspScenario = _scenarioAttributeQuery.Get(smType).ToList();

            if (kspScenario.Any())
            {
                psm.TargetScenes = kspScenario.Single().TargetScenes;
                if (!psm.TargetScenes.Any())
                {
                    _log.Error("KSPScenario TargetScenes attribute is empty - cannot create " + smType.FullName + " because game state would be hosed on next save");
                    throw new Exception("TargetScenes is empty");  // todo: better exception
                }
            }
            else
            {
                // reuse old scenes
                // todo: tell user to update their ScenarioModule in a more blatant way
                _log.Warning("ScenarioModule " + _typeIdentifierQuery.Get(smType) +
                             " does not have KSPScenario attribute. It is recommended to " +
                             "utilize it rather than the legacy ScenarioModule installation of " +
                             "modifying the game's ProtoScenarioModules directly.");

            }

            if (!psm.TargetScenes.Contains(_gameSceneQuery.Get())) return;

            _smFactory.Create(psm, smType);
        }
    }
}
