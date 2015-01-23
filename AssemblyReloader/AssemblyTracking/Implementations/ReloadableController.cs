using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    internal class ReloadableController : IReloadableController
    {
        private readonly IEnumerable<ReloadableAssembly> _reloadables;
        private readonly QueryProvider _queryProvider;
        private readonly ILog _log;

        public ReloadableController(
            QueryProvider queryProvider,
            ILog log,
            params ReloadableAssembly[] reloadables)
        {
            if (queryProvider == null) throw new ArgumentNullException("queryProvider");
            if (log == null) throw new ArgumentNullException("log");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _queryProvider = queryProvider;
            _log = log;
            _reloadables = reloadables;
        }



        public void ReloadAll()
        {
            foreach (var r in _reloadables)
            {
                r.Unload();
                r.Load();
                r.StartAddons(_queryProvider.GetCurrentGameSceneProvider().Get());
            }
        }

        public IEnumerable<IReloadableIdentity> ReloadableAssemblies
        {
            get { return _reloadables.Select(r => r.ReloadableIdentity); }
        }
    }
}