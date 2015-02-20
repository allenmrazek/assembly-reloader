using System;
using AssemblyReloader.Messages;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public interface IAddonLoader : IDisposable, IConsumer<KSPAddon.Startup>
    {
        void LoadForScene(KSPAddon.Startup scene);
    }
}
