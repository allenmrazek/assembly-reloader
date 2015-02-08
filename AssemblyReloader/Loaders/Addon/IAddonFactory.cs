using System;
using AssemblyReloader.Addon;

namespace AssemblyReloader.Loaders.Addon
{
    public interface IAddonFactory
    {
        IDisposable CreateAddon(AddonInfo addonInfo);
        AddonInfo CreateInfoForAddonType(Type type);
    }
}
