using System;
using AssemblyReloader.TypeTracking;

namespace AssemblyReloader.Factory
{
    interface IAddonInfoFactory
    {
        AddonInfo Create(Type type);
    }
}
