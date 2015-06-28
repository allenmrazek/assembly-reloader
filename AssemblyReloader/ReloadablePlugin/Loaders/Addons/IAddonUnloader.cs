using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonUnloader
    {
        void Unload(Assembly assembly);
    }
}
