using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using AssemblyReloader.Annotations;
using AssemblyReloader.Controllers;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Queries.FileSystemQueries;
using ReeperCommon.Serialization;

namespace AssemblyReloader.Commands
{
    public class SaveConfigurationCommand : ICommand
    {
        private readonly IReloadablePlugin _plugin;
        private readonly IConfigNodeFormatter _formatter;
        private readonly IConfigurationFilePathQuery _configPathQuery;

        public SaveConfigurationCommand(
            [NotNull] IReloadablePlugin plugin,
            [NotNull] IConfigNodeFormatter formatter, 
            [NotNull] IConfigurationFilePathQuery configPathQuery)
        {
            if (plugin == null) throw new ArgumentNullException("plugin");
            if (formatter == null) throw new ArgumentNullException("formatter");
            if (configPathQuery == null) throw new ArgumentNullException("configPathQuery");

            _plugin = plugin;
            _formatter = formatter;
            _configPathQuery = configPathQuery;
        }


        public void Execute()
        {
            var config = new ConfigNode();
            var filePath = _configPathQuery.Get(_plugin.Location);

            _formatter.Serialize(_plugin.Configuration, config);

            config.Save(filePath, string.Format("Configuration settings for " + _plugin.Name));
        }
    }
}
