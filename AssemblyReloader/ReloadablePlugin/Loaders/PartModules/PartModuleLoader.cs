extern alias KSP;
using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.injector;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IGetTypesDerivedFrom<KSP::PartModule> _partModuleTypeQuery;
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IGetPartPrefabClones _clonesOfPrefabQuery;
        private readonly ILog _log;


        public PartModuleLoader(
            IGetTypesDerivedFrom<KSP::PartModule> partModuleTypeQuery,
            IPartModuleDescriptorFactory descriptorFactory,
            IPartModuleFactory partModuleFactory,
            IGetPartPrefabClones clonesOfPrefabQuery,
            [Name(LogKey.PartModuleLoader)] ILog log)
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


        ~PartModuleLoader()
        {
            _log.Warning("PartModuleLoader.Destructor");
        }


        public void Load(ILoadedAssemblyHandle handle, bool prefabOnly)
        {
            if (handle == null) throw new ArgumentNullException("handle");

            var descriptions =
                _partModuleTypeQuery.Get(handle.LoadedAssembly.assembly)
                    .SelectMany(pmType => _descriptorFactory.Create(pmType))
                    .ToList();

            _log.Verbose(string.Format("Found {0} PartModule descriptions", descriptions.Count));

            foreach (var description in descriptions)
                CreatePartModulesFromDescription(description, prefabOnly);
        }



        private void CreatePartModulesFromDescription(PartModuleDescriptor description, bool prefabOnly)
        {
            if (description == null) throw new ArgumentNullException("description");

            _log.Debug("Creating PartModules from description " + description);

            // create prefab's PartModule
            _partModuleFactory.Create(description.Prefab, description);

            if (prefabOnly)
                return;

            // todo: (?) if we're not in a scene that has PartModule instances, return an empty list

            // create partmodules for loaded instances of the prefab
            foreach (var loadedInstance in _clonesOfPrefabQuery.Get(description.Prefab))
                _partModuleFactory.Create(loadedInstance, description);
        }
    }
}
