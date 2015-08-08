using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using AssemblyReloader.StrangeIoC.extensions.injector.api;
using Debug = UnityEngine.Debug;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    // This is kind of an ugly way to go about this but there are cases where it's necessary. Specifically,
    // the editor doesn't seem to keep a list of all the parts it spawns and I couldn't find any way to 
    // locate those parts except for a basic FindObjectsOfType call.
// ReSharper disable once ClassNeverInstantiated.Global
    [Implements(typeof(IGetPartPrefabClones), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetPartPrefabClones : IGetPartPrefabClones
    {
        private readonly IGetPartIsPrefab _partIsPrefabQuery;
        private readonly IKspFactory _kspFactory;

        public GetPartPrefabClones(
            IGetPartIsPrefab partIsPrefabQuery,
            IKspFactory kspFactory)
        {
            if (partIsPrefabQuery == null) throw new ArgumentNullException("partIsPrefabQuery");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _partIsPrefabQuery = partIsPrefabQuery;
            _kspFactory = kspFactory;
        }


        public IEnumerable<IPart> Get(IPart prefab)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");
            if (!_partIsPrefabQuery.Get(prefab))
                throw new ArgumentException("argument must be a part prefab");

            var loadedParts = UnityEngine.Object.FindObjectsOfType<Part>();

            return loadedParts
                .Select(p => _kspFactory.Create(p))
                .Where(p => !_partIsPrefabQuery.Get(p));
        }
    }
}
