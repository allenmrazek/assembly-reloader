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
//    public class ScenarioModuleFacade : IReloadableTypeSystem
//    {
//        private readonly IScenarioModuleLoader _loader;
//        private readonly IScenarioModuleUnloader _unloader;
//        private readonly IGetTypesFromAssembly _scenarioModuleQuery;
//        private readonly IGetCurrentGameScene _getCurrentGameScene;

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
//            [NotNull] IGetCurrentGameScene GetCurrentGameScene)
//        {
//            if (loader == null) throw new ArgumentNullException("loader");
//            if (unloader == null) throw new ArgumentNullException("unloader");
//            if (scenarioModuleQuery == null) throw new ArgumentNullException("scenarioModuleQuery");
//            if (GetCurrentGameScene == null) throw new ArgumentNullException("GetCurrentGameScene");

//            _loader = loader;
//            _unloader = unloader;
//            _scenarioModuleQuery = scenarioModuleQuery;
//            _getCurrentGameScene = GetCurrentGameScene;
//        }


//        public void AddToLoadedAssemblies([NotNull] Assembly assembly, [NotNull] IFile location)
//        {
//            if (assembly == null) throw new ArgumentNullException("assembly");
//            if (location == null) throw new ArgumentNullException("location");

//            RunOperationOnScenarioModuleTypes(assembly, _loader.AddToLoadedAssemblies);
//        }


//        public void DestroyReloadableTypesFrom([NotNull] Assembly assembly, [NotNull] IFile location)
//        {
//            if (assembly == null) throw new ArgumentNullException("assembly");
//            if (location == null) throw new ArgumentNullException("location");

//            RunOperationOnScenarioModuleTypes(assembly, _unloader.DestroyReloadableTypesFrom);
//        }


//        private void RunOperationOnScenarioModuleTypes(Assembly assembly, Action<Type> operation)
//        {
//            // if we're not in a scene with ScenarioModules, it stands to reason we have nothing to do here
//            if (!ValidScenarioModuleScenes.Contains(_getCurrentGameScene.Get()))
//                return;

//            foreach (var scenarioModuleType in _scenarioModuleQuery.Get(assembly))
//                operation(scenarioModuleType);
//        }
//    }
//}
