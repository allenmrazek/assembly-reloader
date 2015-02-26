using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;

namespace AssemblyReloader.Providers
{
    class LoadedInstancesOfPrefabProvider : ILoadedInstancesOfPrefabProvider
    {
        private readonly ILoadedVesselProvider _loadedVesselProvider;

        public LoadedInstancesOfPrefabProvider(ILoadedVesselProvider loadedVesselProvider)
        {
            if (loadedVesselProvider == null) throw new ArgumentNullException("loadedVesselProvider");
            _loadedVesselProvider = loadedVesselProvider;
        }


        public IEnumerable<IPart> Get(IPart prefab)
        {
            return _loadedVesselProvider.Get()
                .SelectMany(v => v.Parts.Where(p => ReferenceEquals(p.PartInfo, prefab.PartInfo)));
        }
    }
}
