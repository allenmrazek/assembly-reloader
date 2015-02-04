using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    public class ReloadableController : IReloadableController
    {
        private readonly IEnumerable<IReloadableAssembly> _reloadables;
        private readonly IQueryFactory _queryFactory;


        public ReloadableController(
            IQueryFactory queryFactory,
            IEnumerable<IReloadableAssembly> reloadables)
        {
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _queryFactory = queryFactory;
            _reloadables = reloadables;
        }


        public void Reload(IReloadableAssembly toReload)
        {
            if (toReload == null) throw new ArgumentNullException("toReload");

            toReload.Unload();
            toReload.Load();
            toReload.StartAddons(
                _queryFactory.GetStartupSceneFromGameSceneQuery().Get(
                    _queryFactory.GetCurrentGameSceneProvider().Get())
                    );
        }


        public void ReloadAll()
        {
            foreach (var r in _reloadables)
                Reload(r);
        }


        public IEnumerable<IReloadableAssembly> ReloadableAssemblies
        {
            get { return _reloadables; }
        }
    }
}