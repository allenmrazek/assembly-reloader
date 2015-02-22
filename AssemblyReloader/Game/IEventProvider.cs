namespace AssemblyReloader.Game
{
    public interface IEventProvider
    {
        IGameEventPublisher<GameScenes> OnLevelWasLoaded { get; }
        IGameEventPublisher<KSPAddon.Startup> OnSceneLoaded { get; }
    }
}
