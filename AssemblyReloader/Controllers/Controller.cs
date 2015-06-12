using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Commands;
using AssemblyReloader.Gui;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Controllers
{
    public class Controller : IController
    {
        private readonly Dictionary<IPluginInfo, IReloadablePlugin> _plugins;
        private readonly ICommand _saveConfiguration;
        private readonly ICommand<IPluginInfo> _savePluginConfiguration;
        private readonly ILog _log;


        public Controller(
            [NotNull] Dictionary<IPluginInfo, IReloadablePlugin> plugins,
            [NotNull] ICommand saveConfiguration,
            [NotNull] ICommand<IPluginInfo> savePluginConfiguration,
            [NotNull] ILog log)
        {
            if (plugins == null) throw new ArgumentNullException("plugins");
            if (saveConfiguration == null) throw new ArgumentNullException("saveConfiguration");
            if (savePluginConfiguration == null) throw new ArgumentNullException("savePluginConfiguration");
            if (log == null) throw new ArgumentNullException("log");

            _plugins = plugins;
            _saveConfiguration = saveConfiguration;
            _savePluginConfiguration = savePluginConfiguration;
            _log = log;
        }


        public void Reload([NotNull] IPluginInfo plugin)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");

            var target = GetReloadablePlugin(plugin);
            if (!target.Any())
                throw new Exception("Could not find plugin associated with " + plugin.Name);
            
            Reload(target.Single(), plugin);
        }


        public void SaveConfiguration()
        {
            _saveConfiguration.Execute();
        }


        public void SavePluginConfiguration(IPluginInfo plugin)
        {
            _savePluginConfiguration.Execute(plugin);
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


        private Maybe<IReloadablePlugin> GetReloadablePlugin([NotNull] IPluginInfo info)
        {
            if (info == null) throw new ArgumentNullException("info");

            IReloadablePlugin target;

            return _plugins.TryGetValue(info, out target)
                ? Maybe<IReloadablePlugin>.With(target)
                : Maybe<IReloadablePlugin>.None;

        }
    }
}
