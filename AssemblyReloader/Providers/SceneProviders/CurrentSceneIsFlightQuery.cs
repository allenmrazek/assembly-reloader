namespace AssemblyReloader.Providers.SceneProviders
{
    public class CurrentSceneIsFlightQuery : ICurrentSceneIsFlightQuery
    {
        public bool Get()
        {
            return HighLogic.LoadedSceneIsFlight;
        }
    }
}
