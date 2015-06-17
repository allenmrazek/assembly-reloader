namespace AssemblyReloader.Providers
{
    public interface IFilePathProvider
    {
        string Get();
    }


    public interface IFilePathProvider<TContext>
    {
        string Get(TContext context);
    }
}
