using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    class ReloadableController : IReloadableController
    {
        private readonly IEnumerable<ReloadableAssembly> _reloadables;
        private readonly ILog _log;

        public ReloadableController(
            ILog log,
            params ReloadableAssembly[] reloadables)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _log = log;
            _reloadables = reloadables;
        }



        ~ReloadableController()
        {
            foreach (var reloadable in _reloadables)
                reloadable.Dispose();
        }

        public void ReloadAll()
        {
            foreach (var r in _reloadables)
            {
                r.Unload();
                r.Load();
            }
        }
    }
}
