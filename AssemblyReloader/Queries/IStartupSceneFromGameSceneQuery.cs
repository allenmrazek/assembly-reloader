namespace AssemblyReloader.Queries
{
    public interface IStartupSceneFromGameSceneQuery
    {
        KSPAddon.Startup Get(GameScenes gameScene);
    }
}
