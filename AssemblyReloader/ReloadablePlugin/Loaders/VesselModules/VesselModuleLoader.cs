extern alias KSP;
using System;
using System.Linq;
using ReeperAssemblyLibrary;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
// ReSharper disable once ClassNeverInstantiated.Global
    public class VesselModuleLoader : IVesselModuleLoader
    {
        private readonly IVesselModuleManager _vesselModuleManager;
        private readonly IGetTypesDerivedFrom<KSP::VesselModule> _vesselModules;
        private readonly IGetLoadedVessels _loadedVessels;
        private readonly IVesselModuleSettings _settings;
        private readonly ILog _log;

        public VesselModuleLoader(
            IVesselModuleManager vesselModuleManager,
            IGetTypesDerivedFrom<KSP::VesselModule> vesselModules,
            IGetLoadedVessels loadedVessels,
            IVesselModuleSettings settings,
            ILog log)
        {
            if (vesselModuleManager == null) throw new ArgumentNullException("vesselModuleManager");
            if (vesselModules == null) throw new ArgumentNullException("vesselModules");
            if (loadedVessels == null) throw new ArgumentNullException("loadedVessels");
            if (settings == null) throw new ArgumentNullException("settings");
            if (log == null) throw new ArgumentNullException("log");

            _vesselModuleManager = vesselModuleManager;
            _vesselModules = vesselModules;
            _loadedVessels = loadedVessels;
            _settings = settings;
            _log = log;
        }


        public void Load(ILoadedAssemblyHandle handle)
        {
            foreach (var vesselModuleType in _vesselModules.Get(handle.LoadedAssembly.assembly)
                                                .Select(vmType => new VesselModuleType(vmType)))
                LoadVesselModule(vesselModuleType);
        }


        private void LoadVesselModule(VesselModuleType vesselModule)
        {
            
            InsertVesselModuleWrapper(vesselModule);

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
            _vesselModuleManager.AddWrapper(new KSP::VesselModuleManager.VesselModuleWrapper(vesselModule.Type)
            {
                active = true,
                order = 0
            });
        }


        private void CreateVesselModuleOnVessels(VesselModuleType vesselModule)
        {
            foreach (var vessel in _loadedVessels.Get()
                .Where(vessel => vessel.gameObject.GetComponent(vesselModule.Type) == null))
                    vessel.gameObject.AddComponent(vesselModule.Type);
        }
    }
}
