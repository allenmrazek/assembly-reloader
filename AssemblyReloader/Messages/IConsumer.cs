namespace AssemblyReloader.Messages
{
    public interface IConsumer<in T>
    {
        void Consume(T message);
    }
}
