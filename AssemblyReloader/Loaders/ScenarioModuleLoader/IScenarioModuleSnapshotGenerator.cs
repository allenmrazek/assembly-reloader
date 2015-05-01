using AssemblyReloader.Game;

namespace AssemblyReloader.Loaders.ScenarioModuleLoader
{
    public interface IScenarioModuleSnapshotGenerator
    {
        void Snapshot(ScenarioModule instance, IProtoScenarioModule psm);
    }
}
