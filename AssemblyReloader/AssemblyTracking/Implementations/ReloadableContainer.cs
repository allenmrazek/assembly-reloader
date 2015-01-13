using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.AddonTracking;
using AssemblyReloader.Queries;
using ReeperCommon.Logging;

namespace AssemblyReloader.AssemblyTracking.Implementations
{

    class ReloadableContainer : IReloadableContainer
    {
        private readonly IEnumerable<ReloadableAssembly> _reloadables;
        private readonly Log _log;

        public ReloadableContainer(
            Log log,
            params ReloadableAssembly[] reloadables)
        {
            if (log == null) throw new ArgumentNullException("log");
            if (reloadables == null) throw new ArgumentNullException("reloadables");

            _log = log;
            _reloadables = reloadables;
        }





        public void ReloadAllAssemblies()
        {
            throw new NotImplementedException();

            _log.Normal("Container: Reloading all " + _reloadables.Count() + " assemblies");
        }



        public void UnloadAll()
        {
            //_log.Normal("Container: Unloading all assemblies");
            //foreach (var reloadable in _reloadables)
            //    reloadable.
        }

    }
}
