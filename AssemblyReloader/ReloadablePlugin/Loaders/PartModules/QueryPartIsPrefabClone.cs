using System;
using AssemblyReloader.Game;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    [Implements(typeof(IQueryPartIsPrefabClone), InjectionBindingScope.CROSS_CONTEXT)]
// ReSharper disable once UnusedMember.Global
    public class QueryPartIsPrefabClone : IQueryPartIsPrefabClone
    {
        private readonly IPartPrefabProvider _prefabProvider;
        private readonly IQueryPartIsPrefab _isPrefabQuery;

        public QueryPartIsPrefabClone(
            IPartPrefabProvider prefabProvider,
            IQueryPartIsPrefab isPrefabQuery)
        {
            if (prefabProvider == null) throw new ArgumentNullException("prefabProvider");
            if (isPrefabQuery == null) throw new ArgumentNullException("isPrefabQuery");
            _prefabProvider = prefabProvider;
            _isPrefabQuery = isPrefabQuery;

        }


        public bool Get(IPart queryPart, IPart prefabPart)
        {
            if (queryPart == null) throw new ArgumentNullException("queryPart");
            if (prefabPart == null) throw new ArgumentNullException("prefabPart");
            if (!_isPrefabQuery.Get(prefabPart))
                throw new ArgumentException("Specified prefab part is not a prefab", "prefabPart");

            return !_isPrefabQuery.Get(queryPart) && _prefabProvider.GetPrefab(queryPart).Equals(prefabPart);
        }
    }
}
