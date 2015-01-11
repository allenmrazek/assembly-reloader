namespace AssemblyReloader.Messages
{
    public interface IChannel
    {
        void Send<T>(T message);
    }
}
