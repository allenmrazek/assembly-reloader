using System;
using System.Linq;
using ReeperAssemblyLibrary;
using ReeperCommon.Containers;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class VesselModuleLoader : IVesselModuleLoader
    {
        private readonly IVesselModuleManager _vesselModuleManager;
        private readonly IGetTypesDerivedFrom<VesselModule> _vesselModules;
        private readonly IGetLoadedVessels _loadedVessels;
        private readonly IVesselModuleSettings _settings;
        private readonly IVesselModuleConfigNodeRepository _snapshotRepository;
        private readonly IGetTypeIdentifier _queryTypeIdentifier;
        private readonly ILog _log;

        public VesselModuleLoader(
            IVesselModuleManager vesselModuleManager,
            IGetTypesDerivedFrom<VesselModule> vesselModules,
            IGetLoadedVessels loadedVessels,
            IVesselModuleSettings settings,
            IVesselModuleConfigNodeRepository snapshotRepository, 
            IGetTypeIdentifier queryTypeIdentifier,
            ILog log)
        {
            if (vesselModuleManager == null) throw new ArgumentNullException("vesselModuleManager");
            if (vesselModules == null) throw new ArgumentNullException("vesselModules");
            if (loadedVessels == null) throw new ArgumentNullException("loadedVessels");
            if (settings == null) throw new ArgumentNullException("settings");
            if (queryTypeIdentifier == null) throw new ArgumentNullException("queryTypeIdentifier");
            if (log == null) throw new ArgumentNullException("log");

            _vesselModuleManager = vesselModuleManager;
            _vesselModules = vesselModules;
            _loadedVessels = loadedVessels;
            _settings = settings;
            _snapshotRepository = snapshotRepository;
            _queryTypeIdentifier = queryTypeIdentifier;
            _log = log;
        }


        public void Load(ILoadedAssemblyHandle handle)
        {
            foreach (var vesselModuleType in _vesselModules.Get(handle.LoadedAssembly.assembly)
                .Select(vmType => new VesselModuleType(vmType)))
            {
                InsertVesselModuleWrapper(vesselModuleType);
                LoadVesselModule(vesselModuleType);
            }
        }


        private void LoadVesselModule(VesselModuleType vesselModule)
        {
            if (_settings.CreateVesselModulesImmediately)
                CreateVesselModuleOnVessels(vesselModule);
        }


        private void InsertVesselModuleWrapper(VesselModuleType vesselModule)
        {
            var existingWrapper = _vesselModuleManager.GetModuleWrapper(vesselModule);

            if (existingWrapper.Any())
            {
                _log.Error("A VesselModuleWrapper for " + vesselModule + " already exists");
                return;
            }

            _log.Debug("Inserting " + vesselModule + " wrapper");
            _vesselModuleManager.AddWrapper(new VesselModuleManager.VesselModuleWrapper(vesselModule.Type)
            {
                active = true, // todo: respect old active tag
                order = 0 // todo: we really should scan for the correct order ;\
            });
        }


        private void CreateVesselModuleOnVessels(VesselModuleType vesselModuleType)
        {
            foreach (var vessel in _loadedVessels.Get()
                .Where(vessel => vessel.gameObject.GetComponent(vesselModuleType.Type) == null))
            {
                var vesselModule = vessel.gameObject.AddComponent(vesselModuleType.Type);

                var typeIdentifier = _queryTypeIdentifier.Get(vesselModuleType.Type);

                var snapshot = _snapshotRepository.Retrieve(typeIdentifier, vessel.ID);

                if (!snapshot.Any() || !(vesselModule is VesselModule)) continue;

                try
                {
                    vesselModule.With(vm => vm as VesselModule).Do(vm => vm.Load(snapshot.Value));
                }
                catch (Exception e)
                {
                    Log.Error("Exception while loading " + vesselModuleType + " snapshot: " + e);
                }
            }
        }
    }
}
