using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AssemblyReloader.Game;
using AssemblyReloader.Queries;
using ReeperCommon.Logging.Implementations;

namespace AssemblyReloader.Providers
{
    // This is kind of an ugly way to go about this but there are cases where it's necessary. Specifically,
    // the editor doesn't seem to keep a list of all the parts it spawns and I couldn't find any way to 
    // locate those parts except for a basic FindObjectsOfType call.
    //
    // This is a problem for us; we might end up mixing PartModules from different assemblies when it comes
    // to orphaned Parts (individual or as a cluster) if we miss some loaded prefabs. This is possible in the
    // editor at least, where orphaned parts are transparent (think player temporarily detaching stuff from
    // the ship)
    public class PartPrefabCloneProvider : IPartPrefabCloneProvider
    {
        private readonly ILoadedComponentProvider<Part> _loadedPartProvider;
        private readonly IComponentsInGameObjectHierarchyProvider<Part> _partsInGameObject;
        private readonly IPartIsPrefabQuery _confirmPrefabQuery;
        private readonly IKspFactory _kspFactory;

        public PartPrefabCloneProvider(
            ILoadedComponentProvider<Part> loadedPartProvider,
            IComponentsInGameObjectHierarchyProvider<Part> partsInGameObject,
            IPartIsPrefabQuery confirmPrefabQuery,
            IKspFactory kspFactory)
        {
            if (loadedPartProvider == null) throw new ArgumentNullException("loadedPartProvider");
            if (partsInGameObject == null) throw new ArgumentNullException("partsInGameObject");
            if (confirmPrefabQuery == null) throw new ArgumentNullException("confirmPrefabQuery");
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            _loadedPartProvider = loadedPartProvider;
            _partsInGameObject = partsInGameObject;
            _confirmPrefabQuery = confirmPrefabQuery;
            _kspFactory = kspFactory;
        }


        public IEnumerable<IPart> Get(IPart prefab)
        {
            if (prefab == null) throw new ArgumentNullException("prefab");
            if (!_confirmPrefabQuery.Get(prefab))
                throw new ArgumentException("argument must be a part prefab");


            var loadedParts = _loadedPartProvider.Get();

            // Bit tricky here: loadedParts is looking for loose parts so if the parts are actually attached to each
            // other via parenting instead of joints (such as when building a ship in the editor), we'll only find
            // the top-level ones! Better look for children too
            return loadedParts
                .SelectMany(p => _partsInGameObject.Get(p.gameObject))
                .Select(_kspFactory.Create)
                .Where(p => _confirmPrefabQuery.Get(p));

            //return _loadedPartProvider.Get()
            //    .Select(p => _kspFactory.Create(p))

            //    .Where(p => !_confirmPrefabQuery.Get(p)); // just to protect against us grabbing the prefabs themselves in case a plugin has made them active for who knows why

        }
    }
}
