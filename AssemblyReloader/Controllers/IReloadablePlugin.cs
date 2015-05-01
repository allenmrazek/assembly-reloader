using System.Reflection;
using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public delegate void PluginLoadedHandler(Assembly assembly, IFile location);
    public delegate void PluginUnloadedHandler(Assembly assembly, IFile location);

    public interface IReloadablePlugin
    {
        void Load();
        void Unload();

        string Name { get; }
        Maybe<Assembly> Assembly { get; }
        PluginConfiguration Configuration { get; }
        IFile Location { get; }

        event PluginLoadedHandler OnLoaded;
        event PluginUnloadedHandler OnUnloaded;
    }
}
