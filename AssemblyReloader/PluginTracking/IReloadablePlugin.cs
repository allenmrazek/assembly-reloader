using System.Reflection;
using ReeperCommon.Containers;

namespace AssemblyReloader.PluginTracking
{
    internal delegate void AssemblyReloadedDelegate(Assembly assembly);

    public interface IReloadablePlugin
    {
        void Load();
        void Unload();

        string Name { get; }

        Maybe<Assembly> Assembly { get; }
    }
}
