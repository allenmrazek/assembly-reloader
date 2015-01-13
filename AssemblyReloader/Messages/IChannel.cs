namespace AssemblyReloader.Messages
{
    public interface IChannel
    {
        void Send<T>(T message);

        void AddListener<T>(object listener);
        void RemoveListener(object listener);
    }
}
