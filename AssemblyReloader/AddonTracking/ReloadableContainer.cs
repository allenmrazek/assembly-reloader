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
        private readonly LoaderProvider _loaderProvider;
        private readonly StartupSceneFromGameSceneQuery _sceneQuery;
        private readonly List<ReloadableAssembly> _reloadables = new List<ReloadableAssembly>();
        private readonly Log _log;

        public ReloadableContainer(
            ReloadableAssemblyFactory reloadableAssemblyFactory,
            LoaderProvider loaderProvider,
            AddonInfoFactory infoFactory,
            ReloadableAssemblyFileQuery reloadableAssemblyFileQuery,
            StartupSceneFromGameSceneQuery sceneQuery,
            Log log)
        {
            if (reloadableAssemblyFactory == null) throw new ArgumentNullException("reloadableAssemblyFactory");
            if (loaderProvider == null) throw new ArgumentNullException("loaderProvider");
            if (infoFactory == null) throw new ArgumentNullException("infoFactory");
            if (reloadableAssemblyFileQuery == null) throw new ArgumentNullException("reloadableAssemblyFileQuery");
            if (sceneQuery == null) throw new ArgumentNullException("sceneQuery");
            if (log == null) throw new ArgumentNullException("log");

            _reloadableAssemblyFactory = reloadableAssemblyFactory;
            _loaderProvider = loaderProvider;
            _sceneQuery = sceneQuery;
            _log = log;


            var reloadableFiles = reloadableAssemblyFileQuery.Get().ToList();
            log.Normal("ReloadableContainer: Located {0} reloadable plugins", reloadableFiles.Count.ToString());

            reloadableFiles.ForEach(file => _reloadables.Add(reloadableAssemblyFactory.Create(file, loaderProvider, infoFactory, log)));
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
