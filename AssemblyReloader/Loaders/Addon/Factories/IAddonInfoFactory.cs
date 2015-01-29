using System;
using AssemblyReloader.TypeTracking;

namespace AssemblyReloader.Loaders.Addon.Factories
{
    interface IAddonInfoFactory
    {
        AddonInfo Create(Type type);
    }
}
