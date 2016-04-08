using System;
using System.Collections.ObjectModel;
using System.Linq;
using AssemblyReloader.ReloadablePlugin.Loaders;
using ReeperCommon.Extensions;

namespace AssemblyReloader.Game
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class KspPartLoader : IPartLoader, IPartPrefabProvider
    {
        private readonly IKspFactory _kspFactory;


        public KspPartLoader(IKspFactory kspFactory)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _kspFactory = kspFactory;
        }


        public ReadOnlyCollection<IAvailablePart> LoadedParts
        {
            get
            {
                return (!PartLoader.Instance.IsNull() && !PartLoader.LoadedPartsList.IsNull())
                    ? PartLoader.LoadedPartsList.Select(ap => _kspFactory.Create(ap)).ToList().AsReadOnly()
                    : Enumerable.Empty<IAvailablePart>().ToList().AsReadOnly();
            }
        }


        /// <summary>
        /// Note: this is necessary because in some cases the prefab reported by Part.partInfo.partPrefab
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
        }
    }
}
