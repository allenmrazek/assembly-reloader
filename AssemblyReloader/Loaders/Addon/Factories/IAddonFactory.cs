using System;
using AssemblyReloader.TypeTracking;

namespace AssemblyReloader.Loaders.Addon.Factories
{
    interface IAddonFactory
    {
        IDisposable Create(AddonInfo addonInfo);
    }
}
