using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders.PartModuleLoader
{
    public interface IPartModuleSnapshotGenerator
    {
        void Snapshot(IPart part, PartModule instance);
    }
}
