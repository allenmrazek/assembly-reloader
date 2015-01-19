using System;
using System.Reflection;

namespace AssemblyReloader.AssemblyTracking
{
    internal delegate void AssemblyReloadedDelegate(Assembly assembly);

    interface IReloadableAssembly
    {
        void Load();
        void Unload();
    }
}
