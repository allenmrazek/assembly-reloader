namespace AssemblyReloader.Queries.ConversionQueries
{
    public interface IStartupSceneFromGameSceneQuery
    {
        KSPAddon.Startup Get(GameScenes gameScene);
    }
}
