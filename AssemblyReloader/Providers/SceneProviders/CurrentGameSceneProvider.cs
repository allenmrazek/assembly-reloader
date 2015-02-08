namespace AssemblyReloader.Providers.SceneProviders
{
    public class CurrentGameSceneProvider : ICurrentGameSceneProvider
    {
        public GameScenes Get()
        {
            return HighLogic.LoadedScene;
        }
    }
}
