namespace AssemblyReloader.FileSystem
{
    public interface IFilePathProvider
    {
        string Get();
    }

    public interface IFilePathProvider<in TContext>
    {
        string Get(TContext context);
    }
}
