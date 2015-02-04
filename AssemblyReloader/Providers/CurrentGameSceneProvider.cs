namespace AssemblyReloader.Providers
{
    public class CurrentGameSceneProvider : ICurrentGameSceneProvider
    {
        public GameScenes Get()
        {
            return HighLogic.LoadedScene;
        }
    }
}
