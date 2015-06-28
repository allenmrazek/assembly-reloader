using System.Reflection;

namespace AssemblyReloader.ReloadablePlugin.Loaders.Addons
{
    public interface IAddonLoader
    {
        void Load(Assembly assembly);
    }
}
