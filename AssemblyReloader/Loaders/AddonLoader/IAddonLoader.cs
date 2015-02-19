using System;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public interface IAddonLoader : IDisposable
    {
        void LoadForScene(KSPAddon.Startup scene);
    }
}
