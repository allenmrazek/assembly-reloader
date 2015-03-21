using System;
using System.Reflection;
using AssemblyReloader.Config;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.PluginTracking
{
    public delegate void PluginLoadedHandler(Assembly assembly, IFile location);
    public delegate void PluginUnloadedHandler(Assembly assembly, IFile location);

    public interface IReloadablePlugin
    {
        void Load();
        void Unload();

        string Name { get; }
        Maybe<Assembly> Assembly { get; }
        IConfiguration Configuration { get; }

        event PluginLoadedHandler OnLoaded;
        event PluginUnloadedHandler OnUnloaded;
    }
}
