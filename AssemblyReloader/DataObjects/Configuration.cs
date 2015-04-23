using ReeperCommon.Serialization;

namespace AssemblyReloader.DataObjects
{
    public class Configuration
    {
        [ReeperPersistent]
        public bool StartAddonsForCurrentScene = true;

        [ReeperPersistent]
        public bool ReloadPartModulesImmediately = true;


        [ReeperPersistent]
        public bool IgnoreCurrentSceneForInstantAddons = true;

        [ReeperPersistent]
        public bool RewriteAssemblyLocationCalls = true;
    }
}
