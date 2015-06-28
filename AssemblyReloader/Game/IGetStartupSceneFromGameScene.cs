namespace AssemblyReloader.Game
{
    public interface IGetStartupSceneFromGameScene
    {
        KSPAddon.Startup Get(GameScenes gameScene);
    }
}
