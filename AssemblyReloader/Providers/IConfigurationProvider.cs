using AssemblyReloader.DataObjects;

namespace AssemblyReloader.Providers
{
    public interface IConfigurationProvider
    {
        Configuration Get();
    }
}
