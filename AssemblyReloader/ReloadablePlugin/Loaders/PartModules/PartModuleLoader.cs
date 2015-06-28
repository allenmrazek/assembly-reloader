using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.CompositeRoot;
using AssemblyReloader.DataObjects;
using AssemblyReloader.Game.Providers;
using AssemblyReloader.Properties;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> _configNodeQueue;
        private readonly IPartPrefabCloneProvider _loadedPrefabProvider;
        private readonly Func<bool> _useConfigNodeSnapshotIfAvailable;
        private readonly ILog _log = new DebugLog("PartModuleLoader");

        public PartModuleLoader(
            IPartModuleDescriptorFactory descriptorFactory,
            IPartModuleFactory partModuleFactory,
            DictionaryQueue<KeyValuePair<uint, ITypeIdentifier>, ConfigNode> configNodeQueue,
            IPartPrefabCloneProvider loadedPrefabProvider,
            [NotNull] Func<bool> useConfigNodeSnapshotIfAvailable )
        {
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (configNodeQueue == null) throw new ArgumentNullException("configNodeQueue");
            if (loadedPrefabProvider == null) throw new ArgumentNullException("loadedPrefabProvider");
            if (useConfigNodeSnapshotIfAvailable == null)
                throw new ArgumentNullException("useConfigNodeSnapshotIfAvailable");

            _descriptorFactory = descriptorFactory;
            _partModuleFactory = partModuleFactory;
            _configNodeQueue = configNodeQueue;
            _loadedPrefabProvider = loadedPrefabProvider;
            _useConfigNodeSnapshotIfAvailable = useConfigNodeSnapshotIfAvailable;
        }


        public void Load([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (!type.IsSubclassOf(typeof (PartModule)))
                throw new Exception(type.FullName + " is not a subclass of PartModule");

            _log.Verbose("Loading " + type.FullName);

            var descriptions = _descriptorFactory.Create(type).ToList();

            descriptions.ForEach(LoadPartModule);
        }



        private void LoadPartModule([NotNull] PartModuleDescriptor description)
        {
            if (description == null) throw new ArgumentNullException("description");

            // not included in list because it won't be started
            _partModuleFactory.Create(description.Prefab, description.Type, description.Config);

            foreach (var loadedInstance in _loadedPrefabProvider.Get(description.Prefab))
            {
                var stored = _configNodeQueue.Retrieve(new KeyValuePair<uint, ITypeIdentifier>(loadedInstance.FlightID, description.Identifier));
                var config = _useConfigNodeSnapshotIfAvailable() && stored.Any() ? stored.Single() : description.Config;

                _partModuleFactory.Create(loadedInstance, description.Type, config);
            }
        }
    }
}
