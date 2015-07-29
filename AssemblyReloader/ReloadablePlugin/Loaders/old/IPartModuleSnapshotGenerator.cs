using AssemblyReloader.Game;

namespace AssemblyReloader.ReloadablePlugin.Loaders.old
{
    public interface IPartModuleSnapshotGenerator
    {
        void Snapshot(IPart part, PartModule instance);
    }
}
