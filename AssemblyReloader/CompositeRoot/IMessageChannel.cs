namespace AssemblyReloader.CompositeRoot
{
    public interface IMessageChannel
    {
        void Send<T>(T message);
    }
}
