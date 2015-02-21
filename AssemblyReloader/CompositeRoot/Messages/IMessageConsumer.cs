namespace AssemblyReloader.CompositeRoot.Messages
{
    public interface IMessageConsumer<in T>
    {
        void Consume(T message);
    }
}
