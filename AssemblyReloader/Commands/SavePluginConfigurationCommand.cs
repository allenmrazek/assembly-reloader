//using System;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Controllers;
//using AssemblyReloader.Queries.FileSystemQueries;
//using ReeperCommon.Serialization;

//namespace AssemblyReloader.Commands
//{
//    public class SavePluginConfigurationCommand : ICommand
//    {
//        private readonly IReloadablePlugin _plugin;
//        private readonly IConfigNodeFormatter _formatter;
//        private readonly IPluginConfigurationFilePathQuery _configPathQuery;

//        public SavePluginConfigurationCommand(
//            [NotNull] IReloadablePlugin plugin,
//            [NotNull] IConfigNodeFormatter formatter, 
//            [NotNull] IPluginConfigurationFilePathQuery configPathQuery)
//        {
//            if (plugin == null) throw new ArgumentNullException("plugin");
//            if (formatter == null) throw new ArgumentNullException("formatter");
//            if (configPathQuery == null) throw new ArgumentNullException("configPathQuery");

//            _plugin = plugin;
//            _formatter = formatter;
//            _configPathQuery = configPathQuery;
//        }


//        public void Execute()
//        {
//            var config = new ConfigNode();
//            var filePath = _configPathQuery.Get(_plugin.Location);

//            _formatter.Serialize(_plugin.Configuration, config);

//            config.Save(filePath, string.Format("Configuration settings for " + _plugin.Name));


//        }
//    }
//}
