using System;

namespace AssemblyReloader.Addon
{
    interface IAddonFactory
    {
        IDisposable CreateAddon(AddonInfo addonInfo);
        AddonInfo CreateInfoForAddonType(Type type);
    }
}
