using System.Reflection;

namespace AssemblyReloader.Loaders
{
    public interface IAddonUnloader
    {
        void Unload(Assembly assembly);
    }
}
