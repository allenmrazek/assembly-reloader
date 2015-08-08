using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders;
using ReeperCommon.Extensions;
using ReeperCommon.Logging;

namespace AssemblyReloader.Game
{
    public class KspPartLoader : IPartLoader, IPartLoaderPrefabProvider
    {
        private readonly IKspFactory _kspFactory;
        private readonly ILog _log;
        private readonly Dictionary<string, IPart> _partPrefabs = new Dictionary<string, IPart>();
 

        public KspPartLoader(IKspFactory kspFactory, ILog log)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (log == null) throw new ArgumentNullException("log");

            _kspFactory = kspFactory;
            _log = log;

            try
            {
                _partPrefabs = PartLoader.LoadedPartsList.ToDictionary(ap => ap.name,
                    ap => _kspFactory.Create(ap.partPrefab));
            }
            catch (ArgumentException e)
            {
                // duplicate keys?
                var firstDuplicate = PartLoader.LoadedPartsList
                    .FirstOrDefault(ap => GetPrefabCount(ap.name) > 0);

                if (firstDuplicate != null)
                    throw new DuplicatePartPrefabException(firstDuplicate);

                throw; // unknown
            }
        }


        public List<IAvailablePart> LoadedParts
        {
            get { return (!PartLoader.Instance.IsNull() && !PartLoader.LoadedPartsList.IsNull()) ? PartLoader.LoadedPartsList.Select(ap => _kspFactory.Create(ap)).ToList() : new List<IAvailablePart>(); }
        }


        /// <summary>
        /// Note: this is necessary because in some cases, the prefab reported by Part.partInfo.partPrefab
        /// is NOT the same as the one in PartLoader (specifically, the editor)
        /// </summary>
        /// <param name="from"></param>
        /// <returns></returns>
        public IPart GetPrefab(IPart @from)
        {
            if (@from == null) throw new ArgumentNullException("from");

            var availablePart = PartLoader.getPartInfoByName(@from.PartInfo.Name);

            if (availablePart == null) throw new PrefabNotFoundException(@from);

            return _kspFactory.Create(availablePart.partPrefab);

            //IPart prefab;

            //if (_partPrefabs.TryGetValue(@from.PartInfo.Name, out prefab))
            //    return prefab;

            //_partPrefabs.Keys.ToList().ForEach(prefabName => _log.Debug("Prefab: " + prefabName));



        }


        private int GetPrefabCount(string partName)
        {
            return PartLoader.LoadedPartsList.Count(ap => ap.name == partName);
        }
    }
}
