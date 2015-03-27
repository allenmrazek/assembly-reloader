using System;
using System.Linq;
using AssemblyReloader.Destruction;
using AssemblyReloader.Providers.Game;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Loaders.PartModuleLoader
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

            foreach (var part in descriptions
                .Select(d => d.Prefab)
                .Union(descriptions
                    .SelectMany(d => _loadedInstancesOfPrefabProvider.Get(d.Prefab))))
            {
                var pm = part.GameObject.GetComponent(type) as PartModule;

                if (pm.IsNull()) continue;

                _snapshotGenerator.Snapshot(part, pm);

                part.RemoveModule(pm);

                _partModuleDestroyer.Destroy(pm);
            }
        }
    }
}
