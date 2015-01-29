using System.Reflection;

namespace AssemblyReloader.Loaders.Factories
{
    interface ILoaderFactory
    {
        IAddonLoader CreateAddonLoader(Assembly assembly);
    }
}
