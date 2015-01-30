using System;

namespace AssemblyReloader.Loaders
{
    public interface IAddonLoader : IDisposable
    {
        void LoadAddonsForScene(KSPAddon.Startup scene);
    }
}
