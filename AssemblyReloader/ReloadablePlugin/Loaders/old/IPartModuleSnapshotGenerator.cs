using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules
{
    public interface IPartModuleSnapshotGenerator
    {
        void Snapshot(IPart part, PartModule instance);
    }
}
