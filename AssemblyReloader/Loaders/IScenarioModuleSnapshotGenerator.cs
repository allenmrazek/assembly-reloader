using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders
{
    public interface IScenarioModuleSnapshotGenerator
    {
        void Snapshot(ScenarioModule instance, IProtoScenarioModule psm);
    }
}
