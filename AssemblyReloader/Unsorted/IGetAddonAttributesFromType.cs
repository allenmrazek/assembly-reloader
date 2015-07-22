using System;
using System.Collections.Generic;

namespace AssemblyReloader.Unsorted
{
    public interface IGetAddonAttributesFromType
    {
        IEnumerable<KSPAddon> Get(Type type);
    }
}
