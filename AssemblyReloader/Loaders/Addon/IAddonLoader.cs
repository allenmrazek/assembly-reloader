using System;

namespace AssemblyReloader.Loaders.Addon
{
    public interface IAddonLoader : IDisposable
    {
        void LoadForScene(KSPAddon.Startup scene);
    }
}
