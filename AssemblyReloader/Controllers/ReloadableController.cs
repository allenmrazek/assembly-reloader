using System;
using System.Collections.Generic;
using AssemblyReloader.PluginTracking;
using AssemblyReloader.Providers.SceneProviders;
using AssemblyReloader.Queries;

namespace AssemblyReloader.Controllers
{

    public class ReloadableController : IReloadableController
    {
        private readonly IEnumerable<IReloadablePlugin> _reloadables;
        private readonly IQueryFactory _queryFactory;
        private readonly ICurrentGameSceneProvider _currentSceneProvider;


        public ReloadableController(
            IEnumerable<IReloadablePlugin> reloadables,
            IQueryFactory queryFactory,
            ICurrentGameSceneProvider currentSceneProvider)
        {
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");
            if (currentSceneProvider == null) throw new ArgumentNullException("currentSceneProvider");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _queryFactory = queryFactory;
            _currentSceneProvider = currentSceneProvider;
            _reloadables = reloadables;
        }


        public void Reload(IReloadablePlugin toReload)
        {
            if (toReload == null) throw new ArgumentNullException("toReload");

            toReload.Unload();
            toReload.Load();
        }


        public void ReloadAll()
        {
            foreach (var r in _reloadables)
                Reload(r);
        }


        public IEnumerable<IReloadablePlugin> Plugins
        {
            get { return _reloadables; }
        }
    }
}