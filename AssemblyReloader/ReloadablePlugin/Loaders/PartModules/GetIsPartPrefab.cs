using System;
using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class GetPartIsPrefab : IGetPartIsPrefab
    {
        private readonly IPartLoaderPrefabProvider _prefabProvider;

        public GetPartIsPrefab(IPartLoaderPrefabProvider prefabProvider)
        {
            if (prefabProvider == null) throw new ArgumentNullException("prefabProvider");

            _prefabProvider = prefabProvider;
        }


        public bool Get(IPart part)
        {
            if (part == null) throw new ArgumentNullException("part");

            // see prefab provider implementation for details
            var prefab = _prefabProvider.GetPrefab(part);

            return ReferenceEquals(part.GameObject, prefab.GameObject);
        }
    }
}
