using System;
using System.Collections.Generic;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;

namespace AssemblyReloader.DataObjects
{
    public class Model : IModel
    {
        private readonly IEnumerable<IReloadablePlugin> _plugins;
        private readonly Configuration _configuration;


        public Model([NotNull] Configuration configuration, [NotNull] IEnumerable<IReloadablePlugin> plugins)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (plugins == null) throw new ArgumentNullException("plugins");

            _configuration = configuration;
            _plugins = plugins;
        }


        public IEnumerable<IReloadablePlugin> Plugins
        {
            get { return _plugins; }
        }


        public Configuration Configuration
        {
            get { return _configuration; }
        }
    }
}
