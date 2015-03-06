using AssemblyReloader.DataObjects;
using ReeperCommon.Containers;

namespace AssemblyReloader.Repositories
{
    public interface IFlightConfigRepository
    {
        void Store(uint flightid, ITypeIdentifier key, ConfigNode data);
        Maybe<ConfigNode> Retrieve(uint flightid, ITypeIdentifier key);
        Maybe<ConfigNode> Peek(uint flightid, ITypeIdentifier key); 
        void Clear();
    }
}
