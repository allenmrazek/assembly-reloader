namespace AssemblyReloader.CompositeRoot
{
    public interface IMessageConsumer<T>
    {
        void Consume(T message);
    }
}
