using System.Reflection;

namespace AssemblyReloader.Game
{
    public interface IAddonLoader
    {
        void StartAddons(Assembly assembly, KSPAddon.Startup scene);
    }
}
