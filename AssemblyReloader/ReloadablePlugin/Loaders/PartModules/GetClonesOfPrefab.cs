extern alias KSP;
using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Game;
using strange.extensions.implicitBind;
using strange.extensions.injector.api;
using DestroyAfterTime = KSP::DestroyAfterTime;
using ShipConstruct = KSP::ShipConstruct;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    // This is kind of an ugly way to go about this but there are cases where it's necessary. Specifically,
    // the editor doesn't seem to keep a list of all the parts it spawns and I couldn't find any way to 
    // locate those parts except for a basic FindObjectsOfType call.
// ReSharper disable once ClassNeverInstantiated.Global
    [Implements(typeof(IGetClonesOfPrefab), InjectionBindingScope.CROSS_CONTEXT)]
    public class GetClonesOfPrefab : IGetClonesOfPrefab
    {
        private readonly IQueryPartIsPrefabClone _isPrefabClone;
        private readonly IKspFactory _kspFactory;

        public GetClonesOfPrefab(
            IQueryPartIsPrefabClone isPrefabClone,
            IKspFactory kspFactory)
        {
            if (isPrefabClone == null) throw new ArgumentNullException("isPrefabClone");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");

            _isPrefabClone = isPrefabClone;
            _kspFactory = kspFactory;
        }


        public IEnumerable<IPart> Get(IPart prefab)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");

            var loadedParts = UnityEngine.Object.FindObjectsOfType<KSP::Part>();

            return loadedParts
                .Select(p => _kspFactory.Create(p))
                .Where(p => _isPrefabClone.Get(p, prefab));
        }
    }
}
