using System;
using System.Linq;
using AssemblyReloader.Destruction;
using AssemblyReloader.Game;
using AssemblyReloader.Providers;
using AssemblyReloader.Queries;
using AssemblyReloader.Repositories;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Loaders
{
    public class PartModuleUnloader : IPartModuleUnloader
    {
        private readonly IUnityObjectDestroyer _partModuleDestroyer;
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IPartPrefabCloneProvider _loadedInstancesOfPrefabProvider;
        private readonly IPartModuleSnapshotGenerator _snapshotGenerator;

        public PartModuleUnloader(
            IUnityObjectDestroyer partModuleDestroyer,
            IPartModuleDescriptorFactory descriptorFactory,
            IPartPrefabCloneProvider loadedInstancesOfPrefabProvider,
            IPartModuleSnapshotGenerator snapshotGenerator
            )
        {
            if (partModuleDestroyer == null) throw new ArgumentNullException("partModuleDestroyer");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (loadedInstancesOfPrefabProvider == null)
                throw new ArgumentNullException("loadedInstancesOfPrefabProvider");
            if (snapshotGenerator == null) throw new ArgumentNullException("snapshotGenerator");

            _partModuleDestroyer = partModuleDestroyer;
            _descriptorFactory = descriptorFactory;
            _loadedInstancesOfPrefabProvider = loadedInstancesOfPrefabProvider;
            _snapshotGenerator = snapshotGenerator;
        }


        public void Unload(Type type)
        {
            if (!type.IsSubclassOf(typeof (PartModule)))
                throw new ArgumentException(type.FullName + " is not derived from PartModule");

            var descriptions = _descriptorFactory.Create(type).ToList();

            var log = new DebugLog();

            foreach (var part in descriptions
                .Select(d => d.Prefab)
                .Union(descriptions
                    .SelectMany(d => _loadedInstancesOfPrefabProvider.Get(d.Prefab))))
            {
                log.Normal("Looking for " + type.FullName + " on " + part.PartName);

                var pm = part.GameObject.GetComponent(type) as PartModule;

                if (pm.IsNull()) continue;

                _snapshotGenerator.Snapshot(part, pm);
                _partModuleDestroyer.Destroy(pm);
            }
        }
    }
}
