namespace AssemblyReloader.Providers
{
    public class CurrentSceneIsFlightQuery : ICurrentSceneIsFlightQuery
    {
        public bool Get()
        {
            return HighLogic.LoadedSceneIsFlight;
        }
    }
}
