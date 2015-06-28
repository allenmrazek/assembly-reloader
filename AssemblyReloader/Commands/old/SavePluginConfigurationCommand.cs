//using System;
//using AssemblyReloader.Annotations;
//using AssemblyReloader.Gui;
//using AssemblyReloader.Queries.FileSystemQueries;

//namespace AssemblyReloader.Commands
//{
//    public class SavePluginConfigurationCommand : ICommand<IPluginInfo>
//    {
//        private readonly IGetConfigurationFilePath _configPathQuery;

//        public SavePluginConfigurationCommand(
//            [NotNull] IGetConfigurationFilePath configPathQuery)
//        {
//            if (configPathQuery == null) throw new ArgumentNullException("configPathQuery");

//            _configPathQuery = configPathQuery;
//        }


//        public void Execute(IPluginInfo context)
//        {
//            var filePath = _configPathQuery.Get(context.Location);
//            var config = ConfigNode.CreateConfigFromObject(context.Configuration);

//            if (!config.HasData) throw new Exception("ConfigNode created from configuration is empty");

//            config.Serialize(filePath);
//        }
//    }
//}
