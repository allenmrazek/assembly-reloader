//using System;
//using System.Linq;
//using System.Reflection;
//using ReeperCommon.Containers;
//using ReeperCommon.FileSystem;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.ReloadablePlugin.Weaving
//{
//    public class AssemblyDefinitionWeaver : IAssemblyProvider
//    {
//        private readonly IFile _targetAssembly;
//        private readonly IAssemblyDefinitionProvider _definitionProvider;
//        private readonly IAssemblyDefinitionLoader _definitionLoader;
//        private readonly ILog _log;

//        public AssemblyDefinitionWeaver(
//            IFile targetAssembly, 
//            IAssemblyDefinitionProvider definitionProvider, 
//            IAssemblyDefinitionLoader definitionLoader,
//            ILog log)
//        {
//            if (targetAssembly == null) throw new ArgumentNullException("targetAssembly");
//            if (definitionProvider == null) throw new ArgumentNullException("definitionProvider");
//            if (definitionLoader == null) throw new ArgumentNullException("definitionLoader");
//            if (log == null) throw new ArgumentNullException("log");

//            _targetAssembly = targetAssembly;
//            _definitionProvider = definitionProvider;
//            _definitionLoader = definitionLoader;
//            _log = log;
//        }


//        public Maybe<Assembly> Get()
//        {
//            var definition = _definitionProvider.Get(_targetAssembly);

//            _log.Verbose("Reweaving {0} definition", _targetAssembly.Url);

//            return _definitionLoader.Get(definition.Single());
//        }
//    }
//}
