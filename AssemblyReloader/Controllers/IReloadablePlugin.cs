using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.Controllers
{
    public delegate void PluginLoadedHandler(Assembly assembly, IFile location);
    public delegate void PluginUnloadedHandler(Assembly assembly, IFile location);

    public interface IReloadablePlugin
    {
        void Load();
        void Unload();

        event PluginLoadedHandler OnLoaded;
        event PluginUnloadedHandler OnUnloaded;
    }
}
