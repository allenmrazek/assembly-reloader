extern alias KSP;
using ReeperCommon.Containers;
using VesselModuleManager = KSP::VesselModuleManager;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleManager
    {
        Maybe<VesselModuleManager.VesselModuleWrapper> GetModuleWrapper(VesselModuleType type);

        void AddWrapper(VesselModuleManager.VesselModuleWrapper wrapper);
        void RemoveWrapper(VesselModuleManager.VesselModuleWrapper wrapper);

    }
}
