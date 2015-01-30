using System.Reflection;

namespace AssemblyReloader.Loaders.Factories
{
    public interface ILoaderFactory
    {
        IAddonLoader CreateAddonLoader(Assembly assembly);
    }
}
