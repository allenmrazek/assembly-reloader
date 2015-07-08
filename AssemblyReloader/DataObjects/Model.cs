using System;
using System.Collections.Generic;
using AssemblyReloader.Config;
using AssemblyReloader.Controllers;
using AssemblyReloader.Gui;
using AssemblyReloader.Properties;
using AssemblyReloader.ReloadablePlugin;

namespace AssemblyReloader.DataObjects
{
    public class Model : IModel
    {
        private readonly IDictionary<IPluginInfo, IReloadablePlugin> _plugins;
        private readonly Configuration _configuration;


        public Model([NotNull] Configuration configuration, [NotNull] IDictionary<IPluginInfo, IReloadablePlugin> plugins)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (plugins == null) throw new ArgumentNullException("plugins");

            _configuration = configuration;
            _plugins = plugins;
        }


        public IEnumerable<IPluginInfo> Plugins
        {
            get { return _plugins.Keys; }
        }


        public Configuration Configuration
        {
            get { return _configuration; }
        }


        public bool Reload([NotNull] IPluginInfo which)
        {
            if (which == null) throw new ArgumentNullException("which");

            IReloadablePlugin plugin;

            if (!_plugins.TryGetValue(which, out plugin))
                throw new KeyNotFoundException("No plugin associated with " + which.Name);

            return plugin.Reload();
        }
    }
}
