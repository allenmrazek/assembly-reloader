using ReeperCommon.Containers;

namespace AssemblyReloader.ReloadablePlugin.Loaders.VesselModules
{
    public interface IVesselModuleManager
    {
        Maybe<VesselModuleManager.VesselModuleWrapper> GetModuleWrapper(VesselModuleType type);

        void AddWrapper(VesselModuleManager.VesselModuleWrapper wrapper);
        void RemoveWrapper(VesselModuleManager.VesselModuleWrapper wrapper);

    }
}
