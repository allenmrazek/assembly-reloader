namespace AssemblyReloader.Game
{
    public class GameEventPublisher<T> : IGameEventPublisher<T>
    {
        public event GameEventHandler<T> OnEvent = delegate { };

        public void Raise(T data)
        {
            OnEvent(data);
        }
    }
}
