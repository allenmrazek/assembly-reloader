using System;

namespace AssemblyReloader.AssemblyTracking
{
    interface IReloadableAssembly : IDisposable
    {
        void Load();
        void Unload();
    }
}
