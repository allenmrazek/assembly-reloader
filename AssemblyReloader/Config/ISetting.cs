namespace AssemblyReloader.Config
{
    public interface ISetting<T> : IReadOnlySetting<T>
    {
        void Set(T value);
    }


    public interface IReadOnlySetting<out T>
    {
        T Get();
    }
}
