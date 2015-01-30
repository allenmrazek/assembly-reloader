using System;
using ReeperCommon.Containers;

namespace AssemblyReloader.Queries
{
    public interface IAddonAttributeFromTypeQuery
    {
        Maybe<KSPAddon> Get(Type type);
    }
}
