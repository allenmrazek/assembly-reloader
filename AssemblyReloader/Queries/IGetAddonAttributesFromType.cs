using System;
using System.Collections.Generic;

namespace AssemblyReloader.Queries
{
    public interface IGetAddonAttributesFromType
    {
        IEnumerable<KSPAddon> Get(Type type);
    }
}
