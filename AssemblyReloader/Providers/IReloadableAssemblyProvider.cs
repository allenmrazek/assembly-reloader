using System.Reflection;

namespace AssemblyReloader.Providers
{
    public interface IReloadableAssemblyProvider
    {
        Assembly Get();

        string Name { get; }
    }
}
