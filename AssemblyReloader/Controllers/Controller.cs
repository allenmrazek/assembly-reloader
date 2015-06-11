using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Gui;
using ReeperCommon.Logging;

namespace AssemblyReloader.Controllers
{
    public class Controller : IController
    {
        private readonly Dictionary<IPluginInfo, IReloadablePlugin> _plugins;
        private readonly ICommand _saveConfiguration;
        private readonly ILog _log;


        public Controller([NotNull] Dictionary<IPluginInfo, IReloadablePlugin> plugins,
            [NotNull] ICommand saveConfiguration, [NotNull] ILog log)
        {
            if (plugins == null) throw new ArgumentNullException("plugins");
            if (saveConfiguration == null) throw new ArgumentNullException("saveConfiguration");
            if (log == null) throw new ArgumentNullException("log");

            _plugins = plugins;
            _saveConfiguration = saveConfiguration;
            _log = log;
        }


        public void Reload([NotNull] IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            IReloadablePlugin target;

            if (!_plugins.TryGetValue(plugin, out target))
                throw new KeyNotFoundException("Unable to find " + plugin.Name);
            
            Reload(target, plugin);
        }


        public void SaveConfiguration()
        {
            _saveConfiguration.Execute();
        }


        private void Reload([NotNull] IReloadablePlugin plugin, [NotNull] IPluginInfo info)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (info == null) throw new ArgumentNullException("info");

            try
            {
                plugin.Unload();
            }
            catch (Exception e)
            {
                // todo: spawn popup message?
                _log.Error("Error while unloading plugin " + info.Name + ": " + e);
                throw;
            }


            try
            {
                plugin.Load();
            }
            catch (Exception e)
            {
                // todo: spawn popup message?
                _log.Error("Error while loading plugin " + info.Name + ": " + e);
                throw;
            }
        }
    }
}
