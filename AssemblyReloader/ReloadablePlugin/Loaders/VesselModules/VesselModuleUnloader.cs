extern alias KSP;
using System;
using System.Linq;
using AssemblyReloader.Game;
using ReeperCommon.Logging;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    [Implements(typeof(IVesselModuleUnloader))]
    public class VesselModuleUnloader : IVesselModuleUnloader
    {
        private readonly IGetTypesDerivedFrom<KSP::VesselModule> _vesselModules;
        private readonly IGetLoadedVessels _loadedVessels;
        private readonly IVesselModuleManager _vesselModuleManager;
        private readonly IMonoBehaviourDestroyer _mbDestroyer;
        private readonly ILog _log;


        public VesselModuleUnloader(
            IGetTypesDerivedFrom<KSP::VesselModule> vesselModules,
            IGetLoadedVessels loadedVessels,
            IVesselModuleManager vesselModuleManager,
            IMonoBehaviourDestroyer mbDestroyer,
            ILog log)
        {
            if (vesselModules == null) throw new ArgumentNullException("vesselModules");
            if (loadedVessels == null) throw new ArgumentNullException("loadedVessels");
            if (vesselModuleManager == null) throw new ArgumentNullException("vesselModuleManager");
            if (mbDestroyer == null) throw new ArgumentNullException("mbDestroyer");
            if (log == null) throw new ArgumentNullException("log");

            _vesselModules = vesselModules;
            _loadedVessels = loadedVessels;
            _vesselModuleManager = vesselModuleManager;
            _mbDestroyer = mbDestroyer;
            _log = log;
        }


        public void Unload(ILoadedAssemblyHandle handle)
        {
            foreach (var vesselModuleType in _vesselModules.Get(handle.LoadedAssembly.assembly)
                .Select(vmType => new VesselModuleType(vmType)))
                UnloadVesselModule(vesselModuleType);
        }


        private void UnloadVesselModule(VesselModuleType vesselModule)
        {
            DestroyVesselModuleOnVessels(vesselModule);
            RemoveVesselModuleWrapper(vesselModule);
        }


        private void DestroyVesselModuleOnVessels(VesselModuleType vesselModule)
        {
            foreach (var modules in _loadedVessels.Get()
                .Select(vessel => vessel.gameObject.GetComponents(vesselModule.Type)
                .Where(c => c != null && c.GetType().IsSubclassOf(vesselModule.Type))
                .Cast<KSP::VesselModule>()
                .ToArray()))
            {
                if (!modules.Any())
                    _log.Warning("No VesselModules for " + vesselModule + " found");

                if (modules.Length > 1)
                    _log.Warning("Multiple VesselModules for " + vesselModule + " found");

                // ReSharper disable once ForCanBeConvertedToForeach
                for (int i = 0; i < modules.Length; ++i)
                    _mbDestroyer.DestroyMonoBehaviour(modules[i]);
            }
        }


        private void RemoveVesselModuleWrapper(VesselModuleType vesselModule)
        {
            var wrapper = _vesselModuleManager.GetModuleWrapper(vesselModule);

            if (!wrapper.Any())
            {
                _log.Error("Did not find a VesselModuleWrapper for " + vesselModule + " when it was expected");
                return;
            }

            // these don't seem to be used under normal circumstances; we'll leave a note in the log
            // if that appears to change
            if (wrapper.Single().order != 0)
                _log.Warning(vesselModule + " order = " + wrapper.Single().order);

            if (!wrapper.Single().active)
                _log.Warning(vesselModule + " active = " + wrapper.Single().active);


            _log.Debug("Removing " + vesselModule + " wrapper");
            _vesselModuleManager.RemoveWrapper(wrapper.Single());
        }
    }
}
