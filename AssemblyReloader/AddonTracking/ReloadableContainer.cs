using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using AssemblyReloader.Loaders;
using AssemblyReloader.Providers;

namespace AssemblyReloader.AddonTracking
{

    class ReloadableContainer
    {
        private readonly ReloadableProvider _reloadableProvider;
        private readonly KspStartupSceneFromGameSceneQuery _sceneQuery;

        private readonly List<ReloadableAssembly> _reloadables = new List<ReloadableAssembly>();

        public ReloadableContainer(
            ReloadableProvider reloadableProvider,
            KspStartupSceneFromGameSceneQuery sceneQuery)
        {
            if (reloadableProvider == null) throw new ArgumentNullException("reloadableProvider");
            if (sceneQuery == null) throw new ArgumentNullException("sceneQuery");

            _reloadableProvider = reloadableProvider;
            _sceneQuery = sceneQuery;
        }




        public void HandleLevelLoad(GameScenes scene)
        {
            var addonScene = _sceneQuery.Query(scene);

        }
    }
}
