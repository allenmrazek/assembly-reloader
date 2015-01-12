using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Factory;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.AddonTracking
{

    class ReloadableContainer
    {
        private readonly ReloadableAssemblyFactory _reloadableAssemblyFactory;
        private readonly LoaderFactory _loaderFactory;
        private readonly ReloadableAssemblyFileQuery _reloadableAssemblyFileQuery;
        private readonly StartupSceneFromGameSceneQuery _sceneQuery;
        private readonly List<ReloadableAssembly> _reloadables = new List<ReloadableAssembly>();
        private readonly Log _log;

        public ReloadableContainer(
            ReloadableAssemblyFactory reloadableAssemblyFactory,
            LoaderFactory loaderFactory,
            ReloadableAssemblyFileQuery reloadableAssemblyFileQuery,
            StartupSceneFromGameSceneQuery sceneQuery,
            Log log)
        {
            if (reloadableAssemblyFactory == null) throw new ArgumentNullException("reloadableAssemblyFactory");
            if (loaderFactory == null) throw new ArgumentNullException("loaderFactory");
            if (reloadableAssemblyFileQuery == null) throw new ArgumentNullException("reloadableAssemblyFileQuery");
            if (sceneQuery == null) throw new ArgumentNullException("sceneQuery");
            if (log == null) throw new ArgumentNullException("log");

            _reloadableAssemblyFactory = reloadableAssemblyFactory;
            _loaderFactory = loaderFactory;
            _reloadableAssemblyFileQuery = reloadableAssemblyFileQuery;
            _sceneQuery = sceneQuery;
            _log = log; 
        }



        public void Initialize()
        {
            var reloadableFiles = _reloadableAssemblyFileQuery.Get().ToList();
            _log.Normal("ReloadableContainer: Located {0} reloadable plugins", reloadableFiles.Count.ToString());

            reloadableFiles.ForEach(file =>
            {
                var reloadable = _reloadableAssemblyFactory.Create(file, _loaderFactory, _log);

                reloadable.Initialize();

                _reloadables.Add(reloadable);
            });
        }





        public void HandleLevelLoad(GameScenes scene)
        {
            _log.Normal("Container: handling level load: " + scene);

            var addonScene = _sceneQuery.Query(scene);

            _reloadables.ForEach(reloadable =>
                reloadable.Loaders.ForEach(loader =>
                    loader.DoLevelLoad(addonScene)
                    )
                );
        }
    }
}
