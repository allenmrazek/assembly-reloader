namespace AssemblyReloader.Messages
{
    public interface IConsumer<T>
    {
        void Consume(T message);
    }
}
