using System;
using System.Collections.Generic;

namespace AssemblyReloader.Queries.ConversionQueries
{
    public interface IAddonAttributesFromTypeQuery
    {
        IEnumerable<KSPAddon> Get(Type type);
    }
}
