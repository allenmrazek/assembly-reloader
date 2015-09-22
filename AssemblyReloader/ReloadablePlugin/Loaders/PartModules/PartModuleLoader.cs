extern alias KSP;
using System;
using System.Linq;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;
using strange.extensions.implicitBind;
using strange.extensions.injector;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class PartModuleLoader : IPartModuleLoader
    {
        private readonly IGetTypesDerivedFrom<KSP::PartModule> _partModuleTypeQuery;
        private readonly IPartModuleDescriptorFactory _descriptorFactory;
        private readonly IPartModuleFactory _partModuleFactory;
        private readonly IGetClonesOfPrefab _prefabClonesQuery;
        private readonly IPartModuleSettings _partModuleSettings;
        private readonly ILog _log;


        public PartModuleLoader(
            IGetTypesDerivedFrom<KSP::PartModule> partModuleTypeQuery,
            IPartModuleDescriptorFactory descriptorFactory,
            IPartModuleFactory partModuleFactory,
            IGetClonesOfPrefab prefabClonesQuery,
            IPartModuleSettings partModuleSettings,
            [Name(LogKey.PartModuleLoader)] ILog log)
        {
            if (partModuleTypeQuery == null) throw new ArgumentNullException("partModuleTypeQuery");
            if (descriptorFactory == null) throw new ArgumentNullException("descriptorFactory");
            if (partModuleFactory == null) throw new ArgumentNullException("partModuleFactory");
            if (prefabClonesQuery == null) throw new ArgumentNullException("prefabClonesQuery");
            if (partModuleSettings == null) throw new ArgumentNullException("partModuleSettings");
            if (log == null) throw new ArgumentNullException("log");

            _partModuleTypeQuery = partModuleTypeQuery;
            _descriptorFactory = descriptorFactory;
            _partModuleFactory = partModuleFactory;
            _prefabClonesQuery = prefabClonesQuery;
            _partModuleSettings = partModuleSettings;
            _log = log;
        }


        ~PartModuleLoader()
        {
            _log.Warning("PartModuleLoader.Destructor");
        }


        public void Load(ILoadedAssemblyHandle handle)
        {
            if (handle == null) throw new ArgumentNullException("handle");

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

            _log.Debug("Creating PartModules from description " + description);

            // create prefab's PartModule
            _partModuleFactory.Create(description.Prefab, description);

            if (!_partModuleSettings.CreatePartModulesImmediately)
                return;

            // todo: (?) if we're not in a scene that has PartModule instances, return an empty list

            // create partmodules for loaded instances of the prefab
            foreach (var loadedInstance in _prefabClonesQuery.Get(description.Prefab))
                _partModuleFactory.Create(loadedInstance, description);
        }
    }
}
