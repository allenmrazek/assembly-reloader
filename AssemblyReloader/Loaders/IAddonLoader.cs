using System.Reflection;

namespace AssemblyReloader.Loaders
{
    public interface IAddonLoader
    {
        void Load(Assembly assembly);
    }
}
