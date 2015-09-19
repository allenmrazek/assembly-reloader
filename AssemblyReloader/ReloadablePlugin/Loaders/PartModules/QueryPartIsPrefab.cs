using System;
using AssemblyReloader.Game;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    [Implements(typeof(IQueryPartIsPrefab), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class QueryPartIsPrefab : IQueryPartIsPrefab
    {
        private readonly IPartPrefabProvider _prefabProvider;

        public QueryPartIsPrefab(IPartPrefabProvider prefabProvider)
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
