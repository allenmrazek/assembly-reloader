using System;
using System.Collections.Generic;
using System.Linq;
using AssemblyReloader.Annotations;
using AssemblyReloader.Game;
using AssemblyReloader.Game.Providers;
using ReeperCommon.Logging;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public class PartModuleOnStartRunner : IPartModuleOnStartRunner
    {
        private readonly IKspFactory _kspFactory;
        private readonly IPartModuleStartStateProvider _startStateProvider;
        private readonly IPartPrefabCloneProvider _prefabInstanceProvider;
        private readonly ILog _log = new DebugLog("PartModuleOnStartRunner");

        public PartModuleOnStartRunner(
            [NotNull] IKspFactory kspFactory, 
            [NotNull] IPartModuleStartStateProvider startStateProvider,
            [NotNull] IPartPrefabCloneProvider prefabInstanceProvider)
        {
            if (kspFactory == null) throw new ArgumentNullException("kspFactory");
            if (startStateProvider == null) throw new ArgumentNullException("startStateProvider");
            if (prefabInstanceProvider == null) throw new ArgumentNullException("prefabInstanceProvider");

            _kspFactory = kspFactory;
            _startStateProvider = startStateProvider;
            _prefabInstanceProvider = prefabInstanceProvider;
        }


        public void RunPartModuleOnStarts(
            [NotNull] IEnumerable<PartModuleDescriptor> descriptors)
        {
            if (descriptors == null) throw new ArgumentNullException("descriptors");
            var partModuleDescriptors = descriptors as PartModuleDescriptor[] ?? descriptors.ToArray();

            // determine distinct set of part prefabs from descriptors
            var prefabs = partModuleDescriptors.Select(d => d.Prefab).Distinct(new PartPrefabReferenceComparer()).ToList();
            _log.Debug("There are " + prefabs.Count + " distinct prefab instances we should find");

            foreach (var p in prefabs)
                _log.Debug("Prefab: " + p.PartInfo.Name);

            // these are loaded instances which need OnStart called
            var loadedInstances = prefabs.SelectMany(p => _prefabInstanceProvider.Get(p)).ToList();

            foreach (var i in loadedInstances)
                _log.Debug("Loaded instance: " + i.PartInfo.Name + ", " + i.FlightID);

            _log.Debug("Running OnStart for modules on " + loadedInstances.Count + " separate instances of target prefabs");


            foreach (var descriptor in partModuleDescriptors)
            {
                PartModuleDescriptor descriptor1 = descriptor;
                foreach (var prefabClone in loadedInstances
                    .Where(li => ReferenceEquals(li.PartInfo.PartPrefab.GameObject, descriptor1.Prefab.GameObject)))
                {
                    var pm = prefabClone.GameObject.GetComponent(descriptor.Type) as PartModule;

                    if (pm == null)
                    {
                        _log.Warning("Didn't find PartModule " + descriptor.Type + " on " + prefabClone +
                                     " when it was expected");
                        continue;
                    }

                    var state = _startStateProvider.Get(_kspFactory.Create(pm.vessel));

                    try
                    {
                        _log.Debug("Running OnStart for " + descriptor.Type + " on " + prefabClone);
                        pm.OnStart(state);
                    }
                    catch (Exception e)
                    {
                        _log.Error("Uncaught exception from " + pm.GetType() + " on " + pm.part.partInfo.name + ": " +
                                   e);
                    }
                }
            }
        }
    }
}
