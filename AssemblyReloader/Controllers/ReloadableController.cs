using System;
using System.Collections.Generic;
using AssemblyReloader.PluginTracking;

namespace AssemblyReloader.Controllers
{

    public class ReloadableController : IReloadableController
    {
        private readonly IEnumerable<IReloadablePlugin> _reloadables;


        public ReloadableController(
            IEnumerable<IReloadablePlugin> reloadables)
        {
            if (reloadables == null) throw new ArgumentNullException("reloadables");

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