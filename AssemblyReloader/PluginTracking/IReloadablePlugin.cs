using System.Reflection;

namespace AssemblyReloader.PluginTracking
{
    internal delegate void AssemblyReloadedDelegate(Assembly assembly);

    public interface IReloadablePlugin
    {
        void Load();
        void Unload();
        void StartAddons(KSPAddon.Startup scene);

        string Name { get; }
    }
}
