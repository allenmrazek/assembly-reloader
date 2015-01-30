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
        private readonly ILog _log;

        public ReloadableController(
            IQueryFactory queryFactory,
            ILog log,
            IEnumerable<IReloadableAssembly> reloadables)
        {
            if (queryFactory == null) throw new ArgumentNullException("queryFactory");
            if (log == null) throw new ArgumentNullException("log");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _queryFactory = queryFactory;
            _log = log;
            _reloadables = reloadables;
        }



        public void ReloadAll()
        {
            foreach (var r in _reloadables)
            {
                r.Unload();
                r.Load();
                r.StartAddons(
                    _queryFactory.GetStartupSceneFromGameSceneQuery().Get(
                        _queryFactory.GetCurrentGameSceneProvider().Get())
                        );
            }
        }

        public IEnumerable<IReloadableIdentity> ReloadableAssemblies
        {
            get { return _reloadables.Select(r => r.ReloadableIdentity); }
        }
    }
}