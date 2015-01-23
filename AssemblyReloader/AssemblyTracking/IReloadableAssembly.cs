using System;
using System.Reflection;
using ReeperCommon.FileSystem;

namespace AssemblyReloader.AssemblyTracking
{
    internal delegate void AssemblyReloadedDelegate(Assembly assembly);

    interface IReloadableAssembly
    {
        void Load();
        void Unload();
        IReloadableIdentity ReloadableIdentity { get; }
    }
}
