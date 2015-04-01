namespace AssemblyReloader.CompositeRoot
{
    public interface IMessageConsumer<in T>
    {
        void Consume(T message);
    }
}
