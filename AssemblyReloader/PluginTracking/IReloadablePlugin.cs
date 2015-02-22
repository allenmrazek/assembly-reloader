using System;
using System.Reflection;
using ReeperCommon.Containers;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.PluginTracking
{
    public delegate void PluginLoadedHandler(Assembly assembly);
    public delegate void PluginUnloadedHandler(IFile location);

    public interface IReloadablePlugin
    {
        void Load();
        void Unload();

        string Name { get; }

        event PluginLoadedHandler OnLoaded;
        event PluginUnloadedHandler OnUnloaded;
    }
}
