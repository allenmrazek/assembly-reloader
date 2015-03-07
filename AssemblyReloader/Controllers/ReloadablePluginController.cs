using System;
using System.Collections.Generic;
using AssemblyReloader.Messages;
using AssemblyReloader.PluginTracking;
using ReeperCommon.Logging;

namespace AssemblyReloader.Controllers
{

    public class ReloadablePluginController : IReloadablePluginController
    {
        private readonly IEnumerable<IReloadablePlugin> _reloadables;
        private readonly ILog _log;


        public ReloadablePluginController(
            IEnumerable<IReloadablePlugin> reloadables,
            ILog log)
        {
            if (reloadables == null) throw new ArgumentNullException("reloadables");
            if (log == null) throw new ArgumentNullException("log");

            _reloadables = reloadables;
            _log = log;
        }


        public void Reload(IReloadablePlugin toReload)
        {
            if (toReload == null) throw new ArgumentNullException("toReload");

            _log.Verbose("Reloading " + toReload.Name);

            _log.Verbose("Unloading " + toReload.Name);
            toReload.Unload();

            _log.Verbose("Loading " + toReload.Name);
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