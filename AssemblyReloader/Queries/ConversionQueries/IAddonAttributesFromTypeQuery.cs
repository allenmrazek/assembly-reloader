using System;
using System.Collections.Generic;
using ReeperCommon.Containers;

namespace AssemblyReloader.Queries.ConversionQueries
{
    public interface IAddonAttributesFromTypeQuery
    {
        IEnumerable<KSPAddon> Get(Type type);
    }
}
