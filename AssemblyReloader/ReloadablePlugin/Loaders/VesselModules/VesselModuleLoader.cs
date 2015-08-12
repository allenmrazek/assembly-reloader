extern alias KSP;
using System;
using System.Linq;
using AssemblyReloader.Game;
using AssemblyReloader.StrangeIoC.extensions.implicitBind;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    [Implements(typeof(IVesselModuleLoader))]
    public class VesselModuleLoader : IVesselModuleLoader
    {
        private readonly IVesselModuleManager _vesselModuleManager;
        private readonly IGetTypesDerivedFrom<KSP::VesselModule> _vesselModules;
        private readonly IGetLoadedVessels _loadedVessels;
        private readonly ILog _log;

        public VesselModuleLoader(
            IVesselModuleManager vesselModuleManager,
            IGetTypesDerivedFrom<KSP::VesselModule> vesselModules,
            IGetLoadedVessels loadedVessels,
            ILog log)
        {
            if (vesselModuleManager == null) throw new ArgumentNullException("vesselModuleManager");
            if (vesselModules == null) throw new ArgumentNullException("vesselModules");
            if (loadedVessels == null) throw new ArgumentNullException("loadedVessels");
            if (log == null) throw new ArgumentNullException("log");

            _vesselModuleManager = vesselModuleManager;
            _vesselModules = vesselModules;
            _loadedVessels = loadedVessels;
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
