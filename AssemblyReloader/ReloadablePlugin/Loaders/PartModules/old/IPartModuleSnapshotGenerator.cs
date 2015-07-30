using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.PartModules.old
{
    public interface IPartModuleSnapshotGenerator
    {
        void Snapshot(IPart part, PartModule instance);
    }
}
