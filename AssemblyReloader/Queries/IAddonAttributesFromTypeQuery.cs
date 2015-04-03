using System;
using System.Collections.Generic;

namespace AssemblyReloader.Queries
{
    public interface IAddonAttributesFromTypeQuery
    {
        IEnumerable<KSPAddon> Get(Type type);
    }
}
