using System;
using ReeperCommon.Containers;

namespace AssemblyReloader.Queries.ConversionQueries
{
    public interface IAddonAttributeFromTypeQuery
    {
        Maybe<KSPAddon> Get(Type type);
    }
}
