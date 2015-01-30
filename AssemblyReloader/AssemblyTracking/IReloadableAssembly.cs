using System;
using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.AssemblyTracking
{
    internal delegate void AssemblyReloadedDelegate(Assembly assembly);

    public interface IReloadableAssembly
    {
        void Load();
        void Unload();
        void StartAddons(KSPAddon.Startup scene);

        IReloadableIdentity ReloadableIdentity { get; }
    }
}
