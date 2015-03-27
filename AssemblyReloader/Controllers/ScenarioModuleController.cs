using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries.AssemblyQueries;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public class ScenarioModuleController : IReloadableObjectController
    {
        private readonly IScenarioModuleLoader _loader;
        private readonly IScenarioModuleUnloader _unloader;
        private readonly ITypesFromAssemblyQuery _scenarioModuleQuery;
        private readonly ICurrentGameSceneProvider _currentGameSceneProvider;

        private static readonly GameScenes[] ValidScenarioModuleScenes =
        {
            GameScenes.SPACECENTER,
            GameScenes.FLIGHT,
            GameScenes.EDITOR,
            GameScenes.TRACKSTATION
        };

        public ScenarioModuleController(
            [NotNull] IScenarioModuleLoader loader,
            [NotNull] IScenarioModuleUnloader unloader,
            [NotNull] ITypesFromAssemblyQuery scenarioModuleQuery,
            [NotNull] ICurrentGameSceneProvider currentGameSceneProvider)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (unloader == null) throw new ArgumentNullException("unloader");
            if (scenarioModuleQuery == null) throw new ArgumentNullException("scenarioModuleQuery");
            if (currentGameSceneProvider == null) throw new ArgumentNullException("currentGameSceneProvider");

            _loader = loader;
            _unloader = unloader;
            _scenarioModuleQuery = scenarioModuleQuery;
            _currentGameSceneProvider = currentGameSceneProvider;
        }


        public void Load([NotNull] Assembly assembly, [NotNull] IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");

            // if we're not in a scene with ScenarioModules, it stands to reason we have nothing to do here
            if (!ValidScenarioModuleScenes.Contains(_currentGameSceneProvider.Get()))
                return;

            RunOperationOnScenarioModuleTypes(assembly, _loader.Load);
        }


        public void Unload([NotNull] Assembly assembly, [NotNull] IFile location)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            if (location == null) throw new ArgumentNullException("location");

            RunOperationOnScenarioModuleTypes(assembly, _unloader.Unload);
        }


        private void RunOperationOnScenarioModuleTypes(Assembly assembly, Action<Type> operation)
        {
            foreach (var scenarioModuleType in _scenarioModuleQuery.Get(assembly))
                operation(scenarioModuleType);
        }
    }
}
