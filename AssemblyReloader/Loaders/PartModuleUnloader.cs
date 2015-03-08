using System;
using System.Linq;
using AssemblyReloader.Destruction;
using AssemblyReloader.Providers;
using ReeperCommon.Extensions;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Loaders
{
    public class PartModuleUnloader : IPartModuleUnloader
    {
        private readonly IDestructionController _destroyer;
        private readonly IDescriptorFactory _descriptorFactory;
        private readonly IPartPrefabCloneProvider _loadedInstancesOfPrefabProvider;

        public PartModuleUnloader(
            IDestructionController destroyer,
            IDescriptorFactory descriptorFactory,
            IPartPrefabCloneProvider loadedInstancesOfPrefabProvider
            )
        {
            if (destroyer == null) throw new ArgumentNullException("destroyer");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (loadedInstancesOfPrefabProvider == null)
                throw new ArgumentNullException("loadedInstancesOfPrefabProvider");

            _destroyer = destroyer;
            _descriptorFactory = descriptorFactory;
            _loadedInstancesOfPrefabProvider = loadedInstancesOfPrefabProvider;
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

                if (!pm.IsNull())
                    _destroyer.Destroy(pm);
            }
        }
    }
}
