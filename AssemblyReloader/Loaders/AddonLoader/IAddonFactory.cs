using System;

namespace AssemblyReloader.Loaders.AddonLoader
{
    public interface IAddonFactory
    {
        IDisposable CreateAddon(AddonInfo addonInfo);
        AddonInfo CreateInfoForAddonType(Type type);
    }
}
