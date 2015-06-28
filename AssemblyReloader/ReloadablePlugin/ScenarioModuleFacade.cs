//using System;
//using System.Linq;
//using System.Reflection;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.Game.Providers;
//using AssemblyReloader.Loaders.ScenarioModuleLoader;
//using AssemblyReloader.Queries.AssemblyQueries;
//using ReeperCommon.FileSystem;

//namespace AssemblyReloader.ReloadablePlugin
//{
//    public class ScenarioModuleFacade : IReloadableObjectFacade
//    {
//        private readonly IScenarioModuleLoader _loader;
//        private readonly IScenarioModuleUnloader _unloader;
//        private readonly IGetTypesFromAssembly _scenarioModuleQuery;
//        private readonly ICurrentGameSceneProvider _currentGameSceneProvider;

//        private static readonly GameScenes[] ValidScenarioModuleScenes =
//        {
//            GameScenes.SPACECENTER,
//            GameScenes.FLIGHT,
//            GameScenes.EDITOR,
//            GameScenes.TRACKSTATION
//        };

//        public ScenarioModuleFacade(
//            [NotNull] IScenarioModuleLoader loader,
//            [NotNull] IScenarioModuleUnloader unloader,
//            [NotNull] IGetTypesFromAssembly scenarioModuleQuery,
//            [NotNull] ICurrentGameSceneProvider currentGameSceneProvider)
//        {
//            if (loader == null) throw new ArgumentNullException("loader");
//            if (unloader == null) throw new ArgumentNullException("unloader");
//            if (scenarioModuleQuery == null) throw new ArgumentNullException("scenarioModuleQuery");
//            if (currentGameSceneProvider == null) throw new ArgumentNullException("currentGameSceneProvider");

//            _loader = loader;
//            _unloader = unloader;
//            _scenarioModuleQuery = scenarioModuleQuery;
//            _currentGameSceneProvider = currentGameSceneProvider;
//        }


//        public void Load([NotNull] Assembly assembly, [NotNull] IFile location)
//        {
//            if (assembly == null) throw new ArgumentNullException("assembly");
//            if (location == null) throw new ArgumentNullException("location");

//            RunOperationOnScenarioModuleTypes(assembly, _loader.Load);
//        }


//        public void Unload([NotNull] Assembly assembly, [NotNull] IFile location)
//        {
//            if (assembly == null) throw new ArgumentNullException("assembly");
//            if (location == null) throw new ArgumentNullException("location");

//            RunOperationOnScenarioModuleTypes(assembly, _unloader.Unload);
//        }


//        private void RunOperationOnScenarioModuleTypes(Assembly assembly, Action<Type> operation)
//        {
//            // if we're not in a scene with ScenarioModules, it stands to reason we have nothing to do here
//            if (!ValidScenarioModuleScenes.Contains(_currentGameSceneProvider.Get()))
//                return;

//            foreach (var scenarioModuleType in _scenarioModuleQuery.Get(assembly))
//                operation(scenarioModuleType);
//        }
//    }
//}
