namespace AssemblyReloader.Game
{
    public delegate void GameEventHandler<T>(T data);

    public interface IGameEventPublisher<T>
    {
        event GameEventHandler<T> OnEvent;

        void Raise(T data);
    }
}
