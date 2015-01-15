using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    class ReloadableContainer : IReloadableContainer
    {
        private readonly IEnumerable<ReloadableAssembly> _reloadables;
        private readonly ILog _log;

        public ReloadableContainer(
            ILog log,
            params ReloadableAssembly[] reloadables)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _log = log;
            _reloadables = reloadables;
        }



        ~ReloadableContainer()
        {
            foreach (var reloadable in _reloadables)
                reloadable.Dispose();
        }





    }
}
