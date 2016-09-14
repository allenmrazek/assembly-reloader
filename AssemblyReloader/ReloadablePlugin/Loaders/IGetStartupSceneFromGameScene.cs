namespace AssemblyReloader.ReloadablePlugin.Loaders
{
    public interface IGetStartupSceneFromGameScene
    {
        KSPAddon.Startup Get(GameScenes gameScene);
    }
}
