using System;
using System.ComponentModel;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IGetTypesDerivedFrom<PartModule> _partModuleTypeQuery;
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IGetPartPrefabClones _clonesOfPrefabQuery;
        private readonly ILog _log;


        public PartModuleLoader(
            IGetTypesDerivedFrom<PartModule> partModuleTypeQuery,
            IPartModuleDescriptorFactory descriptorFactory,
            IPartModuleFactory partModuleFactory,
            IGetPartPrefabClones clonesOfPrefabQuery,
            [Name(LogKeys.PartModuleLoader)] ILog log)
        {
            if (partModuleTypeQuery == null) throw new ArgumentNullException("partModuleTypeQuery");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (clonesOfPrefabQuery == null) throw new ArgumentNullException("clonesOfPrefabQuery");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleTypeQuery = partModuleTypeQuery;
            _descriptorFactory = descriptorFactory;
            _partModuleFactory = partModuleFactory;
            _clonesOfPrefabQuery = clonesOfPrefabQuery;
            _log = log;
        }


        public void CreatePartModules(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            var partModuleTypes = _partModuleTypeQuery.Get(handle.LoadedAssembly.assembly);

            var descriptions =
                _partModuleTypeQuery.Get(handle.LoadedAssembly.assembly)
                    .SelectMany(pmType => _descriptorFactory.Create(pmType))
                    .ToList();

            _log.Verbose(string.Format("Found {0} PartModule descriptions", descriptions.Count));

            foreach (var description in descriptions)
                CreatePartModulesFromDescription(description);
        }



        private void CreatePartModulesFromDescription(PartModuleDescriptor description)
        {
            if (description == null) throw new ArgumentNullException("description");

            _log.Debug("Creating PartModules from description " + description.Identifier);

            // create prefab's PartModule
            // (not included in list because it won't be started)
            _partModuleFactory.Create(description.Prefab, description);

            // todo: if we're not in a scene that has PartModule instances, return an empty list

            // create partmodules for loaded instances of the prefab
            foreach (var loadedInstance in _clonesOfPrefabQuery.Get(description.Prefab))
                _partModuleFactory.Create(loadedInstance, description);
        }
    }
}
