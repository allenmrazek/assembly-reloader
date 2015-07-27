//using System;
//using System.Collections.Generic;
//using System.Linq;
//using AssemblyReloader.CompositeRoot;
//using AssemblyReloader.DataObjects;
//using AssemblyReloader.Game;
//using AssemblyReloader.Game.Providers;
//using AssemblyReloader.Properties;
//using ReeperCommon.Logging;

//namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
//{
//    public class PartModuleLoader : IPartModuleLoader
//    {
//        private readonly IPartModuleDescriptorFactory _descriptorFactory;
//        private readonly IPartModuleFactory _partModuleFactory;
//        private readonly ILog _log = new DebugLog("PartModuleLoader");

//        public PartModuleLoader(
//            IPartModuleDescriptorFactory descriptorFactory,
//            IPartModuleFactory partModuleFactory,
//)
//        {
//            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
//            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");


//            _descriptorFactory = descriptorFactory;
//            _partModuleFactory = partModuleFactory;

//        }


//        public void Load(ILoadedAssemblyHandle handle)
//        {
//            if (handle == null) throw new ArgumentNullException("handle");

//            _log.Verbose("Loading PartModules from " + handle.LoadedAssembly.name);

//            _log.Verbose("Loading " + type.FullName);

//            var descriptions = _descriptorFactory.Create(type).ToList();

//            descriptions.ForEach(LoadPartModule);
//        }



//        private void LoadPartModule([NotNull] PartModuleDescriptor description)
//        {
//            if (description == null) throw new ArgumentNullException("description");

//            // not included in list because it won't be started
//            _partModuleFactory.Create(description.Prefab, description.Type, description.Config);

//            foreach (var loadedInstance in _loadedPrefabProvider.Get(description.Prefab))
//            {
//                var stored = _configNodeQueue.Retrieve(new KeyValuePair<uint, ITypeIdentifier>(loadedInstance.FlightID, description.Identifier));
//                var config = _useConfigNodeSnapshotIfAvailable() && stored.Any() ? stored.Single() : description.Config;

//                _partModuleFactory.Create(loadedInstance, description.Type, config);
//            }
//        }
//    }
//}
