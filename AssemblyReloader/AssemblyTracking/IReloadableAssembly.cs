using System;

namespace AssemblyReloader.AssemblyTracking
{
    interface IReloadableAssembly
    {
        void Load();
        void Unload();
    }
}
